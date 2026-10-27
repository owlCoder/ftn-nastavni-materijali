using Oib.Vezba07.Domain.Shared;
using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Application.Ports;

public interface IAccessReviewSchedule
{
    Result<bool> Schedule(AccessReviewItem? item);
}
