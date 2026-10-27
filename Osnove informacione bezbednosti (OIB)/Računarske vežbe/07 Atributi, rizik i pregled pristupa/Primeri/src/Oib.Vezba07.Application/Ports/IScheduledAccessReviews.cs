using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Application.Ports;

public interface IScheduledAccessReviews
{
    IReadOnlyList<AccessReviewItem> Items { get; }
}
