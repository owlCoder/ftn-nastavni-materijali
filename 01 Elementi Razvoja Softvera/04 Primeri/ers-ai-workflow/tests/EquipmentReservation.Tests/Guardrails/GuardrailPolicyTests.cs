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

    [Test]
    public void SensitiveFileGuardrail_BlocksEnvFile()
    {
        var guardrail = new SensitiveFileGuardrail();

        var result = guardrail.Evaluate(new ToolInvocation("Read", null, "/repo/.env"));

        Assert.That(result.Allowed, Is.False);
    }
}
