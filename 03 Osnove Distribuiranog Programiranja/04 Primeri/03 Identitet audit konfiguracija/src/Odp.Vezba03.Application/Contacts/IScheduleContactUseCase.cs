using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Domain.Operations;

namespace Odp.Vezba03.Application.Contacts;

public interface IScheduleContactUseCase
{
    ScheduleContactResult Schedule(ContactRequest request, OperationContext context);
}
