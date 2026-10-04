using NUnit.Framework;
using Odp.Vezba07.Domain.Inbox;

namespace Odp.Vezba07.Tests.Domain;

public sealed class InboxFilterTests
{
    private readonly InboxFilter _filter = new();

    [TestCase(false, InboxCodes.Applied)]
    [TestCase(true, InboxCodes.DuplicateIgnored)]
    public void Decide_AppliesAMessageOnlyTheFirstTime(bool alreadyApplied, string code)
    {
        Assert.That(_filter.Decide(alreadyApplied), Is.EqualTo(code));
    }
}
