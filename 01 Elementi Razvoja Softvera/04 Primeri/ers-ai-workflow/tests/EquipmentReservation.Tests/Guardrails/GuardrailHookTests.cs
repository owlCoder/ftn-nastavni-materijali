using EquipmentReservation.Guardrails.Hosting;
using EquipmentReservation.Guardrails.Parsing;
using EquipmentReservation.Guardrails.Policies;
using EquipmentReservation.Guardrails.Services;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Guardrails;

public sealed class GuardrailHookTests
{
    private readonly GuardrailHook _hook = new(
        new HookInputParser(),
        new GuardrailEvaluator([
            new SensitiveFileGuardrail(),
            new DangerousCommandGuardrail()
        ]));

    [Test]
    public async Task Run_WhenInvocationIsSafe_ReturnsAllowExitCode()
    {
        var (exitCode, error) = await RunAsync(
            """{"tool_name":"run_command","tool_input":{"command":"git status","file_path":null}}""");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(exitCode, Is.EqualTo(HookExitCodes.Allow));
            Assert.That(error, Is.Empty);
        }
    }

    [Test]
    public async Task Run_WhenCommandIsDestructive_BlocksAndExplainsWhy()
    {
        var (exitCode, error) = await RunAsync(
            """{"tool_name":"run_command","tool_input":{"command":"git push --force"}}""");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(exitCode, Is.EqualTo(HookExitCodes.Block));
            Assert.That(error, Does.Contain("blocked by project policy"));
        }
    }

    [Test]
    public async Task Run_WhenFileIsSensitive_Blocks()
    {
        var (exitCode, _) = await RunAsync(
            """{"tool_name":"read_file","tool_input":{"command":null,"file_path":"config/.env"}}""");

        Assert.That(exitCode, Is.EqualTo(HookExitCodes.Block));
    }

    [Test]
    public async Task Run_WhenInputCannotBeParsed_FailsClosed()
    {
        var (exitCode, error) = await RunAsync("this is not json");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(exitCode, Is.EqualTo(HookExitCodes.Block));
            Assert.That(error, Does.Contain("could not be evaluated"));
        }
    }

    private async Task<(int ExitCode, string Error)> RunAsync(string input)
    {
        using var error = new StringWriter();
        var exitCode = await _hook.RunAsync(new StringReader(input), error);
        return (exitCode, error.ToString());
    }
}
