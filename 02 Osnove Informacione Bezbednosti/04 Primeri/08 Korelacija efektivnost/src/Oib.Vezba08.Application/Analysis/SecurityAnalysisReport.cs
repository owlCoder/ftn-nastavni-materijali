using Oib.Vezba08.Domain.Correlation;
using Oib.Vezba08.Domain.Effectiveness;

namespace Oib.Vezba08.Application.Analysis;

public sealed record SecurityAnalysisReport(
    IReadOnlyList<CorrelationCase> Cases,
    ControlEffectiveness Effectiveness);
