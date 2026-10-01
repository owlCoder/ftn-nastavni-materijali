using EquipmentReservation.Api.Contracts;

namespace EquipmentReservation.Api.Endpoints;

public static class ServiceEndpoints
{
    public static IEndpointRouteBuilder MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", (ServiceInfoResponse info) => Results.Ok(info));
        app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
        return app;
    }
}
