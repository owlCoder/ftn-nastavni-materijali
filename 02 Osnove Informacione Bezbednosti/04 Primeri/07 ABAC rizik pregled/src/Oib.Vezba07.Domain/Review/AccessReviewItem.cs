namespace Oib.Vezba07.Domain.Review;

public sealed record AccessReviewItem(
    string SubjectId,
    string Permission,
    string BusinessOwner,
    DateTimeOffset ReviewDue);
