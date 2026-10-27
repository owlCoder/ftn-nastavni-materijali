using Oib.Vezba03.Application.Drift;
using Oib.Vezba03.Domain.Configuration;
using Oib.Vezba03.Domain.Drift;
using Oib.Vezba03.Domain.Drift.Controls;
using Oib.Vezba03.Infrastructure.Configuration;
using Oib.Vezba03.Infrastructure.Correlation;
using Oib.Vezba03.Infrastructure.Logging;

namespace Oib.Vezba03.ConsoleUi;

public static class CompositionRoot
{
    private static readonly SecurityBaseline Baseline =
        new("2026.1", MinimumPasswordLength: 14, RequireMfaForAdmins: true);

    private static readonly SecurityConfiguration ActiveConfiguration =
        new(MinimumPasswordLength: 10, RequireMfaForAdmins: false);

    public static IDetectConfigurationDriftUseCase CreateUseCase(TextWriter output) =>
        new DetectConfigurationDriftHandler(
            new InMemorySecurityBaselineProvider(Baseline),
            new InMemoryActiveConfigurationProvider(ActiveConfiguration),
            new ConfigurationDriftDetector(
            [
                new MinimumPasswordLengthControl(),
                new AdminMfaControl()
            ]),
            new GuidCorrelationIdGenerator(),
            new TextDriftReportLog(output));
}
