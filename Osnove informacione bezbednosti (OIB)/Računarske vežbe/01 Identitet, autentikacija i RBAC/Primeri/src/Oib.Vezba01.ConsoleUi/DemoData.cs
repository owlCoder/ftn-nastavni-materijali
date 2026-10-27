using Oib.Vezba01.Domain.Authorization;
using Oib.Vezba01.Domain.Identity;

namespace Oib.Vezba01.ConsoleUi;

public static class DemoData
{
    public const string ViewReports = "reports:view";
    public const string ExportReports = "reports:export";

    public static IReadOnlyList<Role> Roles { get; } =
    [
        new Role("Operator", new HashSet<string> { ViewReports }),
        new Role("SecurityAdmin", new HashSet<string> { ViewReports, ExportReports })
    ];

    public static Actor Operator { get; } =
        new("ana", IsAuthenticated: true, new HashSet<string> { "Operator" });

    public static Actor AnonymousVisitor { get; } =
        new("nepoznat", IsAuthenticated: false, new HashSet<string> { "SecurityAdmin" });
}
