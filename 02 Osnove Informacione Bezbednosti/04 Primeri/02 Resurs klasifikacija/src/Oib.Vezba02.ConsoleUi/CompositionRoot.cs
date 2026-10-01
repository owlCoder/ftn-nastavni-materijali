using Oib.Vezba02.Application.Resources.Read;
using Oib.Vezba02.Domain.Access;
using Oib.Vezba02.Infrastructure.Audit;
using Oib.Vezba02.Infrastructure.Resources;

namespace Oib.Vezba02.ConsoleUi;

public static class CompositionRoot
{
    public static ResourceAccessDemo CreateDemo(TextWriter output)
    {
        var auditLog = new InMemoryResourceAccessAuditLog();
        var readResource = new ReadResourceHandler(
            new InMemoryProtectedResourceRepository(DemoData.Resources),
            new ResourceReadPolicy(),
            auditLog);

        return new ResourceAccessDemo(readResource, auditLog, output);
    }
}
