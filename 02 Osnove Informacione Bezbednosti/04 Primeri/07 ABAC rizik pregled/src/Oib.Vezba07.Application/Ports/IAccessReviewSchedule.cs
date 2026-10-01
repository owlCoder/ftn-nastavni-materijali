using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Application.Ports;

public interface IAccessReviewSchedule
{
    void Schedule(AccessReviewItem item);
}
