using Oib.Vezba01.Application.Access;
using Oib.Vezba01.Domain.Authorization;
using Oib.Vezba01.Infrastructure.Audit;
using Oib.Vezba01.Infrastructure.Authorization;
using Oib.Vezba01.Infrastructure.Time;

namespace Oib.Vezba01.ConsoleUi;

public static class CompositionRoot
{
    public static RbacDemo CreateDemo(TextWriter output)
    {
        var auditLog = new InMemoryAccessAuditLog();
        var authorizeAccess = new AuthorizeAccessHandler(
            new InMemoryRoleCatalog(DemoData.Roles),
            new RbacPolicy(),
            auditLog,
            new SystemClock());

        return new RbacDemo(authorizeAccess, auditLog, output);
    }
}
