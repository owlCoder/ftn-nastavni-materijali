using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Reservations;
using EquipmentReservation.Guardrails.Services;
using EquipmentReservation.Infrastructure.Persistence;
using EquipmentReservation.Mcp.Workspace;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Architecture;

public sealed class DependencyRuleTests
{
    private const string SolutionPrefix = "EquipmentReservation.";
    private const string Domain = "EquipmentReservation.Domain";
    private const string Application = "EquipmentReservation.Application";

    [Test]
    public void Domain_DoesNotDependOnAnyOtherProject()
    {
        Assert.That(ProjectReferencesOf(typeof(Reservation)), Is.Empty);
    }

    [Test]
    public void Application_DependsOnlyOnDomain()
    {
        Assert.That(
            ProjectReferencesOf(typeof(CreateReservationHandler)),
            Is.EquivalentTo(new[] { Domain }));
    }

    [Test]
    public void Infrastructure_DependsOnlyOnApplicationAndDomain()
    {
        Assert.That(
            ProjectReferencesOf(typeof(InMemoryReservationRepository)),
            Is.SubsetOf(new[] { Application, Domain }));
    }

    [Test]
    public void DevelopmentTooling_StaysOutsideTheBusinessCore()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(ProjectReferencesOf(typeof(GuardrailEvaluator)), Is.Empty);
            Assert.That(ProjectReferencesOf(typeof(ProjectRoot)), Is.Empty);
        }
    }

    private static string[] ProjectReferencesOf(Type typeFromAssembly) =>
        typeFromAssembly.Assembly
            .GetReferencedAssemblies()
            .Select(reference => reference.Name ?? string.Empty)
            .Where(name => name.StartsWith(SolutionPrefix, StringComparison.Ordinal))
            .ToArray();
}
