using NUnit.Framework;
using Oib.Vezba08.Domain.Effectiveness;

namespace Oib.Vezba08.Tests.Domain;

public sealed class ControlEffectivenessCalculatorTests
{
    private readonly ControlEffectivenessCalculator _calculator = new(targetRatio: 0.5m);

    [Test]
    public void Evaluate_ReturnsShareOfBlockedAttempts()
    {
        var effectiveness = _calculator.Evaluate(new ControlMeasurement("step-up", 4, 3));

        Assert.That(effectiveness.Ratio, Is.EqualTo(0.75m));
    }

    [TestCase(4, 1, false)]
    [TestCase(4, 2, true)]
    [TestCase(4, 4, true)]
    public void Evaluate_ComparesMeasuredRatioWithTarget(int attempts, int blocked, bool meetsTarget)
    {
        var effectiveness = _calculator.Evaluate(
            new ControlMeasurement("step-up", attempts, blocked));

        Assert.That(effectiveness.MeetsTarget, Is.EqualTo(meetsTarget));
    }

    [Test]
    public void Evaluate_WhenControlWasNeverExercised_DoesNotClaimItWorks()
    {
        var effectiveness = _calculator.Evaluate(new ControlMeasurement("step-up", 0, 0));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(effectiveness.Ratio, Is.Zero);
            Assert.That(effectiveness.MeetsTarget, Is.False);
        }
    }

    [Test]
    public void Evaluate_WhenMeasurementIsInconsistent_RejectsIt()
    {
        Action evaluate = () => _calculator.Evaluate(new ControlMeasurement("step-up", 2, 3));

        Assert.That(evaluate, Throws.TypeOf<ArgumentOutOfRangeException>());
    }
}
