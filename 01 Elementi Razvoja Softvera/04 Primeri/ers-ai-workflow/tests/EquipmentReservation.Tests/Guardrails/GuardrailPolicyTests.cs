using EquipmentReservation.Guardrails.Models;
using EquipmentReservation.Guardrails.Policies;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Guardrails;

public sealed class GuardrailPolicyTests
{
    [TestCase("git push --force origin main")]
    [TestCase("rm -rf ./src")]
    public void DangerousCommandGuardrail_BlocksDestructiveCommands(string command)
    {
        var guardrail = new DangerousCommandGuardrail();

        var result = guardrail.Evaluate(new ToolInvocation("Bash", command, null));

        Assert.That(result.Allowed, Is.False);
    }

    [TestCase("git status")]
    [TestCase("dotnet test EquipmentReservation.sln")]
    public void DangerousCommandGuardrail_AllowsOrdinaryCommands(string command)
    {
        var guardrail = new DangerousCommandGuardrail();

        var result = guardrail.Evaluate(new ToolInvocation("Bash", command, null));

        Assert.That(result.Allowed, Is.True);
    }

    [TestCase("/repo/.env")]
    [TestCase(".env.production")]
    [TestCase("C:\\repo\\config\\secrets.json")]
    public void SensitiveFileGuardrail_BlocksSecretFiles(string filePath)
    {
        var guardrail = new SensitiveFileGuardrail();

        var result = guardrail.Evaluate(new ToolInvocation("Read", null, filePath));

        Assert.That(result.Allowed, Is.False);
    }

    [TestCase("src/EquipmentReservation.Domain/Reservations/Reservation.cs")]
    [TestCase("docs/environment.md")]
    public void SensitiveFileGuardrail_AllowsOrdinaryFiles(string filePath)
    {
        var guardrail = new SensitiveFileGuardrail();

        var result = guardrail.Evaluate(new ToolInvocation("Read", null, filePath));

        Assert.That(result.Allowed, Is.True);
    }
}
