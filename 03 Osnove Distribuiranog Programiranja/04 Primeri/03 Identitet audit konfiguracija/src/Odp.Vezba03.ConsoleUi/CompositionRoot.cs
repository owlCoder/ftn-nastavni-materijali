using Odp.Vezba03.Application.Contacts;
using Odp.Vezba03.Domain.Configuration;
using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Infrastructure.Audit;
using Odp.Vezba03.Infrastructure.Stations;
using Odp.Vezba03.Infrastructure.Time;

namespace Odp.Vezba03.ConsoleUi;

public static class CompositionRoot
{
    /// <summary>Neispravna konfiguracija zaustavlja pokretanje umesto da tiho promeni ponašanje.</summary>
    public static (ContactDemo? Demo, string? Error) TryCreateDemo(ContactLimits limits, TextWriter output)
    {
        var configuration = new ContactLimitsValidator().Validate(limits);
        if (!configuration.Success)
            return (null, configuration.Error);

        var auditLog = new InMemoryAuditLog();
        var stationGateway = new InMemoryStationGateway(DemoData.AvailableStations);
        var scheduleContact = new ScheduleContactHandler(
            new ContactRequestValidator(limits),
            stationGateway,
            auditLog,
            new SystemClock());

        return (new ContactDemo(scheduleContact, stationGateway, auditLog, output), null);
    }
}
