using System.Text;
using Oib.Vezba08.ConsoleUi;

Console.OutputEncoding = Encoding.UTF8;

var report = CompositionRoot.CreateUseCase().Analyze(DemoData.Control);

Console.WriteLine("Korelisani slučajevi:");
foreach (var correlationCase in report.Cases)
    Console.WriteLine(
        $"{correlationCase.CorrelationId}: broj događaja {correlationCase.EventCount} " +
        $"({string.Join(", ", correlationCase.EventTypes)})");

Console.WriteLine();
Console.WriteLine(
    $"Kontrola {report.Effectiveness.Control}: efektivnost {report.Effectiveness.Ratio:P0}, " +
    $"cilj {DemoData.TargetRatio:P0} — " +
    $"{(report.Effectiveness.MeetsTarget ? "ispunjen" : "nije ispunjen")}");
