using System.Text;
using Oib.Vezba03.ConsoleUi;

Console.OutputEncoding = Encoding.UTF8;

var result = CompositionRoot.CreateUseCase(Console.Out).Detect();
if (!result.Success || result.Value is null)
{
    Console.WriteLine($"Provera nije uspela: {result.Error}");
    return;
}
var report = result.Value;

Console.WriteLine();
Console.WriteLine(report.IsCompliant
    ? "Konfiguracija je usklađena sa referentnim stanjem."
    : $"Broj odstupanja od referentnog stanja {report.BaselineVersion}: {report.Findings.Count}");
