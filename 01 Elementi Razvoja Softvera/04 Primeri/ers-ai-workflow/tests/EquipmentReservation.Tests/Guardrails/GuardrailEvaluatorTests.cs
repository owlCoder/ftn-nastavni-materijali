using EquipmentReservation.Guardrails.Abstractions;
using EquipmentReservation.Guardrails.Models;
using EquipmentReservation.Guardrails.Services;
using Moq;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Guardrails;

public sealed class GuardrailEvaluatorTests
{
    private static readonly ToolInvocation Invocation = new("Bash", "git status", null);

    [Test]
    public void Evaluate_WhenEveryGuardrailAllows_AllowsInvocation()
    {
        var evaluator = new GuardrailEvaluator([
            GuardrailReturning(GuardrailDecision.Allow()),
            GuardrailReturning(GuardrailDecision.Allow())
        ]);

        var decision = evaluator.Evaluate(Invocation);

        Assert.That(decision.Allowed, Is.True);
    }

    [Test]
    public void Evaluate_WhenAnyGuardrailBlocks_ReturnsItsReason()
    {
        var evaluator = new GuardrailEvaluator([
            GuardrailReturning(GuardrailDecision.Allow()),
            GuardrailReturning(GuardrailDecision.Block("Blocked by the second policy."))
        ]);

        var decision = evaluator.Evaluate(Invocation);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(decision.Allowed, Is.False);
            Assert.That(decision.Reason, Is.EqualTo("Blocked by the second policy."));
        }
    }

    private static IToolGuardrail GuardrailReturning(GuardrailDecision decision)
    {
        var guardrail = new Mock<IToolGuardrail>();
        guardrail.Setup(policy => policy.Evaluate(It.IsAny<ToolInvocation>())).Returns(decision);
        return guardrail.Object;
    }
}
