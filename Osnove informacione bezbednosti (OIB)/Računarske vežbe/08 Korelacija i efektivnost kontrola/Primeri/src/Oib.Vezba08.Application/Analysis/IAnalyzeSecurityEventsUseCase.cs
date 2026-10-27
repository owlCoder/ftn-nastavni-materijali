using Oib.Vezba08.Domain.Shared;
namespace Oib.Vezba08.Application.Analysis;
public interface IAnalyzeSecurityEventsUseCase
{
    Result<SecurityAnalysisReport> Analyze(string? control);
}
