using System.Text;
using Odp.Vezba03.ConsoleUi;

Console.OutputEncoding = Encoding.UTF8;

var startup = CompositionRoot.TryCreateDemo(DemoData.Limits, Console.Out);
if (startup.Demo is null)
{
    Console.Error.WriteLine($"Konfiguracija nije ispravna: {startup.Error}");
    return 1;
}

startup.Demo.Run();
return 0;
