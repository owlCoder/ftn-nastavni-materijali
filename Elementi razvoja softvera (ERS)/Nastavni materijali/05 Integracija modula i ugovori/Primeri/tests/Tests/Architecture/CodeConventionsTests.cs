using System.Text.Json;
using EquipmentReservation.Api.Dtos;
using EquipmentReservation.Application.Inventory.Models;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Domain.Reservations;
using EquipmentReservation.Mcp.Workspace;
using NUnit.Framework;

namespace EquipmentReservation.Tests.Architecture;

public sealed class CodeConventionsTests
{
    [Test]
    public void ProductionCode_DoesNotExplicitlyThrowOrReturnNull()
    {
        var root = ProjectRootLocator.Find(TestContext.CurrentContext.TestDirectory).FullPath;
        foreach (string file in Directory.EnumerateFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories))
        {
            string source = File.ReadAllText(file);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(source, Does.Not.Match(@"\bthrow\s+(?:new\s+)?[\w(]"), file);
                Assert.That(source, Does.Not.Match(@"\breturn\s+null\s*;"), file);
                Assert.That(source, Does.Not.Match(@"\brecord\b|\bwith\s*\{"), file);
            }
        }
    }

    [Test]
    public void ApplicationInterfaces_ContainOnlyInterfaces()
    {
        var root = ProjectRootLocator.Find(TestContext.CurrentContext.TestDirectory).FullPath;
        var folder = Path.Combine(root, "src", "Application", "Interfaces");
        foreach (string file in Directory.EnumerateFiles(folder, "*.cs", SearchOption.AllDirectories))
        {
            string source = File.ReadAllText(file);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(Path.GetFileName(file), Does.StartWith("I"), file);
                Assert.That(source, Does.Contain("public interface "), file);
                Assert.That(source, Does.Not.Match(@"\bclass\s+"), file);
            }
        }
    }

    [Test]
    public void BusinessModelLivesInDomain_UseCaseInApplication_DtosInApi()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(typeof(Reservation).Assembly, Is.EqualTo(typeof(InventoryItem).Assembly));
            Assert.That(typeof(CreateReservationCommand).Assembly, Is.EqualTo(typeof(CreateReservationHandler).Assembly));
            Assert.That(typeof(ReserveInventoryRequest).Assembly, Is.EqualTo(typeof(CreateReservationHandler).Assembly));
            Assert.That(typeof(CreateReservationRequestDto).Assembly, Is.Not.EqualTo(typeof(Reservation).Assembly));
        }
    }

    [Test]
    public void Dto_DefaultsAreInitializedEvenForEmptyJson()
    {
        var request = JsonSerializer.Deserialize<CreateReservationRequestDto>("{}");
        var response = JsonSerializer.Deserialize<CreateReservationResponseDto>("{}");
        var health = JsonSerializer.Deserialize<HealthStatusDto>("{}");
        using (Assert.EnterMultipleScope())
        {
            Assert.That(request?.RequestId, Is.EqualTo(Guid.Empty));
            Assert.That(response?.ReservationId, Is.EqualTo(Guid.Empty));
            Assert.That(response?.Outcome, Is.Empty);
            Assert.That(response?.ErrorCode, Is.Empty);
            Assert.That(health?.Status, Is.EqualTo("ok"));
        }
    }
}
