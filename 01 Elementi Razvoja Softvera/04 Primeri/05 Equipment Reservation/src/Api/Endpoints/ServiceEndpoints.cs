using EquipmentReservation.Api.Dtos;

namespace EquipmentReservation.Api.Endpoints;

public static class ServiceEndpoints
{
    public static IEndpointRouteBuilder MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", (ServiceInfoDto info) => Results.Ok(info));
        app.MapGet("/health", () => Results.Ok(new HealthStatusDto()));
        return app;
    }
}
