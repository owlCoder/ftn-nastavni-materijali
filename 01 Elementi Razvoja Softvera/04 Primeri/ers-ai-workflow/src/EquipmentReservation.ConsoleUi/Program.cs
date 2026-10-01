using System.Text;
using EquipmentReservation.ConsoleUi.Composition;
using EquipmentReservation.ConsoleUi.Terminal;
using EquipmentReservation.Infrastructure.Inventory;

Console.OutputEncoding = Encoding.UTF8;

var terminal = new SystemTerminal();
terminal.WriteLine("Equipment Reservation — Console UI");
terminal.WriteLine($"Demo equipment ID: {DemoInventory.EquipmentId}");

await CompositionRoot.CreateMenu(terminal).RunAsync(CancellationToken.None);
