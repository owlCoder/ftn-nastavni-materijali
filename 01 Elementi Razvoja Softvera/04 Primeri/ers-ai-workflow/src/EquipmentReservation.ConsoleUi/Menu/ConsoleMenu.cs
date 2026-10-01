using EquipmentReservation.ConsoleUi.Terminal;

namespace EquipmentReservation.ConsoleUi.Menu;

public sealed class ConsoleMenu
{
    private const string ExitKey = "0";

    private readonly ITerminal _terminal;
    private readonly IReadOnlyList<IMenuAction> _actions;

    public ConsoleMenu(ITerminal terminal, IEnumerable<IMenuAction> actions)
    {
        _terminal = terminal ?? throw new ArgumentNullException(nameof(terminal));
        _actions = actions?.ToArray() ?? throw new ArgumentNullException(nameof(actions));
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ShowOptions();

            var choice = _terminal.ReadLine()?.Trim();
            if (choice is null or ExitKey)
                return;

            var action = _actions.FirstOrDefault(candidate => candidate.Key == choice);
            if (action is null)
                _terminal.WriteLine("Nepoznata opcija.");
            else
                await action.ExecuteAsync(cancellationToken);
        }
    }

    private void ShowOptions()
    {
        _terminal.WriteLine();
        foreach (var action in _actions)
            _terminal.WriteLine($"{action.Key} - {action.Label}");

        _terminal.WriteLine($"{ExitKey} - Izlaz");
        _terminal.Write("Izbor: ");
    }
}
