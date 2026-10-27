using Oib.Vezba07.Domain.Shared;
using Oib.Vezba07.Application.Ports;
using Oib.Vezba07.Domain.Review;

namespace Oib.Vezba07.Infrastructure.Review;

public sealed class InMemoryAccessReviewSchedule : IAccessReviewSchedule, IScheduledAccessReviews
{
    private readonly List<AccessReviewItem> _items = [];

    public IReadOnlyList<AccessReviewItem> Items => _items;

    public Result<bool> Schedule(AccessReviewItem? item)
    {
        if (item is null) return Result<bool>.Fail("InvalidAccessReviewItem");
        _items.Add(item);
        return Result<bool>.Ok(true);
    }
}
