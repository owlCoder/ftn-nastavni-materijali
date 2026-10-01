using Oib.Vezba04.Domain.Risk;

namespace Oib.Vezba04.Application.Assessment;

public sealed record ThreatModelReport(
    IReadOnlyList<ThreatAssessment> Assessments,
    IReadOnlyList<RejectedScenario> Rejected);
