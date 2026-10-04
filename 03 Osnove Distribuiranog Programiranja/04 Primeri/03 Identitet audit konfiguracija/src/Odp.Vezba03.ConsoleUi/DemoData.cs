using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Domain.Operations;

namespace Odp.Vezba03.ConsoleUi;

public static class DemoData
{
    public static readonly ContactLimits Limits = new(TimeSpan.FromMinutes(15));

    public static readonly string[] AvailableStations = ["GS-NOVI-SAD"];

    public static readonly OperationContext Operator = new("op-0001", "ana");

    public static readonly OperationContext Planner = new("op-0002", "marko");

    public static readonly OperationContext NightShift = new("op-0003", "jelena");
}
