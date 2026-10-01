namespace Oib.Vezba08.Application.Analysis;

public interface IAnalyzeSecurityEventsUseCase
{
    SecurityAnalysisReport Analyze(string control);
}
