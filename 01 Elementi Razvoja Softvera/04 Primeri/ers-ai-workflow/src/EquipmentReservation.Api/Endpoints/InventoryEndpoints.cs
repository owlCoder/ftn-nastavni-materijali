using EquipmentReservation.Api.Contracts;
using EquipmentReservation.Application.Inventory.GetAvailability;

namespace EquipmentReservation.Api.Endpoints;

public static class InventoryEndpoints
{
    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory/{equipmentId:guid}", GetAvailabilityAsync);
        return app;
    }

    private static async Task<IResult> GetAvailabilityAsync(
        Guid equipmentId,
        IGetEquipmentAvailabilityUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(
            new GetEquipmentAvailabilityQuery(equipmentId),
            cancellationToken);

        return result.Success
            ? Results.Ok(new InventoryAvailabilityResponse(
                result.Value.EquipmentId,
                result.Value.Available))
            : Results.NotFound(new ErrorResponse(result.Error));
    }
}
