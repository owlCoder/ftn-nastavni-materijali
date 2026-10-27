using Odp.Vezba01.Domain.Liveness;

namespace Odp.Vezba01.Application.Monitoring;

public sealed record NodeStatusView(string NodeId, string StationId, NodeLivenessAssessment Liveness);
