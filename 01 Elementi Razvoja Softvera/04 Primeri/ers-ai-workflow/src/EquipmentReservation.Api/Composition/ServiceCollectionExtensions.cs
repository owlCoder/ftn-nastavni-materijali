using EquipmentReservation.Api.Contracts;
using EquipmentReservation.Application.Common;
using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Application.Ports.Reservations;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Domain.Inventory;
using EquipmentReservation.Infrastructure.Concurrency;
using EquipmentReservation.Infrastructure.Identity;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;

namespace EquipmentReservation.Api.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEquipmentReservation(this IServiceCollection services) =>
        services
            .AddDomainServices()
            .AddUseCases()
            .AddInMemoryAdapters()
            .AddServiceInfo();

    private static IServiceCollection AddDomainServices(this IServiceCollection services) =>
        services.AddSingleton<InventoryReservationService>();

    private static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services
            .AddSingleton<IValidator<CreateReservationCommand>, CreateReservationCommandValidator>()
            .AddScoped<ICreateReservationUseCase, CreateReservationHandler>()
            .AddScoped<IGetEquipmentAvailabilityUseCase, GetEquipmentAvailabilityHandler>();

    private static IServiceCollection AddInMemoryAdapters(this IServiceCollection services) =>
        services
            .AddSingleton(provider => new InMemoryInventoryModule(
                provider.GetRequiredService<InventoryReservationService>(),
                DemoInventory.Items))
            .AddSingleton<IInventoryModule>(provider =>
                provider.GetRequiredService<InMemoryInventoryModule>())
            .AddSingleton<IInventoryReadModel>(provider =>
                provider.GetRequiredService<InMemoryInventoryModule>())
            .AddSingleton<IReservationRepository, InMemoryReservationRepository>()
            .AddSingleton<IReservationRequestLock, InMemoryReservationRequestLock>()
            .AddSingleton<IReservationIdGenerator, GuidReservationIdGenerator>();

    private static IServiceCollection AddServiceInfo(this IServiceCollection services) =>
        services.AddSingleton(new ServiceInfoResponse(
            "Equipment Reservation teaching example",
            [
                "GET /health",
                "GET /inventory/{equipmentId}",
                "POST /reservations"
            ],
            DemoInventory.EquipmentId));
}
