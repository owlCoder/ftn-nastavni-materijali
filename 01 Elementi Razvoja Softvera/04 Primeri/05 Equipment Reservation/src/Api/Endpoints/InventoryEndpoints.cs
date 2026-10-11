using EquipmentReservation.Application.Inventory.GetAvailability;
using EquipmentReservation.Application.Interfaces.UseCases;
using EquipmentReservation.Api.Dtos;
using EquipmentReservation.Domain.Inventory;

namespace EquipmentReservation.Api.Endpoints;

public static class InventoryEndpoints
{
    public static IEndpointRouteBuilder MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory/{equipmentId:guid}", GetAvailabilityAsync);
        return app;
    }

    private static async Task<IResult> GetAvailabilityAsync(Guid equipmentId, IGetEquipmentAvailabilityUseCase useCase, CancellationToken cancellationToken)
    {
        var result = await useCase.HandleAsync(new GetEquipmentAvailabilityQuery(equipmentId), cancellationToken);
        if (result.Success)
            return Results.Ok(new InventoryAvailabilityDto { EquipmentId = result.Value.EquipmentId, Available = result.Value.Available });
        if (result.Error is InventoryErrorCodes.InvalidEquipment or InventoryErrorCodes.RequestCancelled)
            return Results.BadRequest(new ErrorDto { Error = result.Error });

        return Results.NotFound(new ErrorDto { Error = result.Error });
    }
}
