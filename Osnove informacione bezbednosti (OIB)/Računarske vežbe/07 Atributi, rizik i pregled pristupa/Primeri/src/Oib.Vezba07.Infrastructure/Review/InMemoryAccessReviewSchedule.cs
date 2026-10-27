using Oib.Vezba07.Application.Ports;
using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Infrastructure.Review;

public sealed class InMemoryAccessReviewSchedule : IAccessReviewSchedule, IScheduledAccessReviews
{
    private readonly List<AccessReviewItem> _items = [];

    public IReadOnlyList<AccessReviewItem> Items => _items;

    public void Schedule(AccessReviewItem item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }
}
