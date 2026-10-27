using System.Text;
using Oib.Vezba06.ConsoleUi;

Console.OutputEncoding = Encoding.UTF8;

var incidents = CompositionRoot.CreateUseCase().Detect();

Console.WriteLine($"Otvoreno incidenata: {incidents.Count}");
foreach (var incident in incidents)
    Console.WriteLine(
        $"{incident.Id} | {incident.Severity} | {incident.Summary} | {incident.Status}");
