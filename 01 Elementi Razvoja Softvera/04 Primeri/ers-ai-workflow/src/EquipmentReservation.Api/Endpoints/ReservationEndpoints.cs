using EquipmentReservation.Api.Contracts;
using EquipmentReservation.Application.Reservations.Create;

namespace EquipmentReservation.Api.Endpoints;

public static class ReservationEndpoints
{
    public static IEndpointRouteBuilder MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/reservations", CreateAsync);
        return app;
    }

    private static async Task<IResult> CreateAsync(
        CreateReservationRequest request,
        ICreateReservationUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateReservationCommand(
            request.RequestId,
            request.EquipmentId,
            request.StudentId,
            request.Quantity);

        var result = await useCase.HandleAsync(command, cancellationToken);

        return result.Outcome switch
        {
            CreateReservationOutcome.Confirmed => Results.Ok(ToResponse(result, "confirmed")),
            CreateReservationOutcome.Rejected => Results.Conflict(ToResponse(result, "rejected")),
            CreateReservationOutcome.Invalid => Results.BadRequest(
                new ErrorResponse(result.ErrorCode ?? "InvalidRequest")),
            _ => throw new InvalidOperationException("Unsupported reservation outcome.")
        };
    }

    private static CreateReservationResponse ToResponse(
        CreateReservationResult result,
        string outcome) =>
        new(
            result.ReservationId
                ?? throw new InvalidOperationException("A decided reservation must have an id."),
            outcome,
            result.ErrorCode,
            result.Replayed);
}
