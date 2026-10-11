using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Application.Interfaces.UseCases;
using EquipmentReservation.Api.Dtos;

namespace EquipmentReservation.Api.Endpoints;

public static class ReservationEndpoints
{
    public static IEndpointRouteBuilder MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/reservations", CreateAsync);
        return app;
    }

    private static async Task<IResult> CreateAsync(CreateReservationRequestDto request, ICreateReservationUseCase useCase, CancellationToken cancellationToken)
    {
        var command = new CreateReservationCommand(request.RequestId, request.EquipmentId, request.StudentId, request.Quantity);
        var result = await useCase.HandleAsync(command, cancellationToken);

        return result.Outcome switch
        {
            CreateReservationOutcome.Confirmed => Results.Ok(ToDto(result, "confirmed")),
            CreateReservationOutcome.Rejected => Results.Conflict(ToDto(result, "rejected")),
            _ when result.ErrorCode == CreateReservationErrorCodes.RequestPayloadMismatch => Results.Conflict(new ErrorDto { Error = result.ErrorCode }),
            _ when result.ErrorCode is CreateReservationErrorCodes.PersistenceFailed or CreateReservationErrorCodes.CompensationFailed =>
                Results.Json(new ErrorDto { Error = result.ErrorCode }, statusCode: StatusCodes.Status503ServiceUnavailable),
            _ => Results.BadRequest(new ErrorDto { Error = result.ErrorCode })
        };
    }

    private static CreateReservationResponseDto ToDto(CreateReservationResult result, string outcome) => new()
    {
        ReservationId = result.ReservationId,
        Outcome = outcome,
        ErrorCode = result.ErrorCode,
        Replayed = result.Replayed
    };
}
