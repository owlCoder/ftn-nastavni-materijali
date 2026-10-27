using System.Text;
using Oib.Vezba06.ConsoleUi;

Console.OutputEncoding = Encoding.UTF8;

var result = CompositionRoot.CreateUseCase().Detect();
if (!result.Success || result.Value is null)
{
    Console.WriteLine($"Detekcija nije uspela: {result.Error}");
    return;
}
var incidents = result.Value;

Console.WriteLine($"Otvoreno incidenata: {incidents.Count}");
foreach (var incident in incidents)
    Console.WriteLine(
        $"{incident.Id} | {incident.Severity} | {incident.Summary} | {incident.Status}");
