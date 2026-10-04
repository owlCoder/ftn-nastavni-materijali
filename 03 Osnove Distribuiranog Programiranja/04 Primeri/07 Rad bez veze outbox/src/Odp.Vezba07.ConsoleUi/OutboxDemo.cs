using Odp.Vezba07.Application.Flushing;
using Odp.Vezba07.Application.Recording;
using Odp.Vezba07.Infrastructure.Center;
using Odp.Vezba07.Infrastructure.Links;

namespace Odp.Vezba07.ConsoleUi;

public sealed class OutboxDemo(
    IRecordMeasurementUseCase record,
    IFlushOutboxUseCase flush,
    SimulatedUplink uplink,
    CenterInbox center,
    TextWriter output)
{
    public void Run()
    {
        uplink.IsUp = false;
        output.WriteLine("Veza je u prekidu.");
        Record("m-1", "signal -91 dBm");
        Record("m-2", "signal -88 dBm");
        Record("m-3", "signal -97 dBm");
        Flush("pokušaj slanja bez veze");

        uplink.IsUp = true;
        uplink.LoseNextAck = true;
        output.WriteLine("Veza se vratila, ali se prva potvrda gubi.");
        Flush("prvi pokušaj posle povratka");
        Flush("drugi pokušaj");

        output.WriteLine();
        output.WriteLine($"Centar je primenio {center.AppliedPayloads.Count} poruke redom:");
        foreach (var payload in center.AppliedPayloads)
            output.WriteLine($"  {payload}");
        output.WriteLine($"Ignorisanih duplikata: {center.DuplicatesIgnored}");
    }

    private void Record(string messageId, string payload) =>
        output.WriteLine($"  merenje {messageId} -> {record.Record(messageId, payload)}");

    private void Flush(string label)
    {
        var report = flush.Flush();
        output.WriteLine($"  {label} -> {report.Code}, isporučeno {report.Delivered}, preostalo {report.Remaining}");
    }
}
