namespace EquipmentReservation.ConsoleUi.Terminal;

public sealed class SystemTerminal : ITerminal
{
    public void Write(string text) => Console.Write(text);

    public void WriteLine(string text = "") => Console.WriteLine(text);

    public string? ReadLine() => Console.ReadLine();
}
