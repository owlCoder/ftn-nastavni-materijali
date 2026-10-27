namespace EquipmentReservation.ConsoleUi.Interfaces;

public interface ITerminal
{
    void Write(string text);
    void WriteLine(string text = "");
    string ReadLine();
}
