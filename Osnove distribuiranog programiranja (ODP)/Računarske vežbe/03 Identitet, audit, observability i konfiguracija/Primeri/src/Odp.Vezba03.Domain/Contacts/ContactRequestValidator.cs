using Odp.Vezba03.Domain.Shared;

namespace Odp.Vezba03.Domain.Contacts;

public sealed class ContactRequestValidator(ContactLimits limits)
{
    public Result Validate(ContactRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.MissionId))
            return Result.Fail(ContactCodes.MissionIdRequired);
        if (string.IsNullOrWhiteSpace(request.StationId))
            return Result.Fail(ContactCodes.StationIdRequired);
        if (request.Duration <= TimeSpan.Zero)
            return Result.Fail(ContactCodes.DurationMustBePositive);
        if (request.Duration > limits.MaxDuration)
            return Result.Fail(ContactCodes.DurationTooLong);

        return Result.Ok();
    }
}
