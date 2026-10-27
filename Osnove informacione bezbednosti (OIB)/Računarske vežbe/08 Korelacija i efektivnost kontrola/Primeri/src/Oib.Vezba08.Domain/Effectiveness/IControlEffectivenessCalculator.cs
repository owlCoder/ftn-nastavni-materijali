using Oib.Vezba08.Domain.Shared;
namespace Oib.Vezba08.Domain.Effectiveness;
public interface IControlEffectivenessCalculator
{
    Result<ControlEffectiveness> Evaluate(ControlMeasurement? measurement);
}
