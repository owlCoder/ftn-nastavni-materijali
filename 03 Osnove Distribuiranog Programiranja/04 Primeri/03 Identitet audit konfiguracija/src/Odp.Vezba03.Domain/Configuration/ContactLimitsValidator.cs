using Odp.Vezba03.Domain.Contacts;
using Odp.Vezba03.Domain.Shared;

namespace Odp.Vezba03.Domain.Configuration;

public sealed class ContactLimitsValidator
{
    public static readonly TimeSpan HardLimit = TimeSpan.FromHours(1);

    public Result Validate(ContactLimits limits)
    {
        ArgumentNullException.ThrowIfNull(limits);

        if (limits.MaxDuration <= TimeSpan.Zero)
            return Result.Fail(ConfigurationCodes.MaxDurationMustBePositive);
        if (limits.MaxDuration > HardLimit)
            return Result.Fail(ConfigurationCodes.MaxDurationAboveHardLimit);

        return Result.Ok();
    }
}
