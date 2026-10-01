namespace EquipmentReservation.ConsoleUi.Menu;

public interface IMenuAction
{
    string Key { get; }
    string Label { get; }
    Task ExecuteAsync(CancellationToken cancellationToken);
}
