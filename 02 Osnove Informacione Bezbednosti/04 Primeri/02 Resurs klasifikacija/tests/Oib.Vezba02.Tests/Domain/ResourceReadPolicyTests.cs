using NUnit.Framework;
using Oib.Vezba02.Domain.Access;
using Oib.Vezba02.Domain.Resources;

namespace Oib.Vezba02.Tests.Domain;

public sealed class ResourceReadPolicyTests
{
    private readonly ResourceReadPolicy _policy = new();

    [Test]
    public void Decide_WhenRequesterOwnsResource_Allows()
    {
        var decision = _policy.Decide(
            Requester("ana", Permissions.ReadRecords),
            Resource(DataClassification.Restricted));

        Assert.That(decision, Is.EqualTo(new ResourceAccessDecision(true, ResourceAccessCodes.OwnerAccess)));
    }

    [Test]
    public void Decide_WhenRequesterIsNotOwner_Denies()
    {
        var decision = _policy.Decide(
            Requester("marko", Permissions.ReadRecords),
            Resource(DataClassification.Internal));

        Assert.That(decision, Is.EqualTo(new ResourceAccessDecision(false, ResourceAccessCodes.NotOwner)));
    }

    [Test]
    public void Decide_WhenOwnerLacksReadPermission_Denies()
    {
        var decision = _policy.Decide(Requester("ana"), Resource(DataClassification.Internal));

        Assert.That(
            decision,
            Is.EqualTo(new ResourceAccessDecision(false, ResourceAccessCodes.MissingReadPermission)));
    }

    [TestCase(DataClassification.Internal)]
    [TestCase(DataClassification.Confidential)]
    public void Decide_WhenElevatedRequesterReadsNonRestrictedResource_Allows(
        DataClassification classification)
    {
        var decision = _policy.Decide(
            Requester("jelena", Permissions.ReadRecords, Permissions.ReadAnyRecord),
            Resource(classification));

        Assert.That(
            decision,
            Is.EqualTo(new ResourceAccessDecision(true, ResourceAccessCodes.ElevatedAccess)));
    }

    [Test]
    public void Decide_WhenElevatedRequesterReadsRestrictedResource_Denies()
    {
        var decision = _policy.Decide(
            Requester("jelena", Permissions.ReadRecords, Permissions.ReadAnyRecord),
            Resource(DataClassification.Restricted));

        Assert.That(
            decision,
            Is.EqualTo(new ResourceAccessDecision(false, ResourceAccessCodes.RestrictedToOwner)));
    }

    private static Requester Requester(string actorId, params string[] permissions) =>
        new(actorId, permissions.ToHashSet());

    private static ProtectedResource Resource(DataClassification classification) =>
        new("rec-42", "ana", classification);
}
