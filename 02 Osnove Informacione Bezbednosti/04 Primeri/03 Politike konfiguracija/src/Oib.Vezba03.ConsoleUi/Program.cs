using System.Text;
using Oib.Vezba03.ConsoleUi;

Console.OutputEncoding = Encoding.UTF8;

var report = CompositionRoot.CreateUseCase(Console.Out).Detect();

Console.WriteLine();
Console.WriteLine(report.IsCompliant
    ? "Konfiguracija je usklađena sa referentnim stanjem."
    : $"Broj odstupanja od referentnog stanja {report.BaselineVersion}: {report.Findings.Count}");
