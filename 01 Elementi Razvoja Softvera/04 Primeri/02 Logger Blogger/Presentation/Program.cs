using Presentation.Composition;
using Presentation.Console;
using Shared;

// Skladište se bira na jednom mestu (factory), umesto da se "pogađa" tokom pokretanja.
// Dozvoljene vrednosti: "in-memory" (podrazumevano) ili "json".
AppContainer container = AppContainer.Create("in-memory");

Result<Unit> seedResult = container.Seeder.Seed();
if (seedResult.IsFailure)
{
    System.Console.WriteLine($"Seed greška: {seedResult.Error.Code} - {seedResult.Error.Message}");
    return;
}

ConsoleMenu meni = new(container.Io, container);
meni.Run();
