using System.Text.Json;
using System.Text.Json.Serialization;
using EquipmentReservation.Api.Contracts;
using EquipmentReservation.Application.Ports.Inventory;
using EquipmentReservation.Application.Ports.Reservations;
using EquipmentReservation.Application.Reservations.Create;
using EquipmentReservation.Infrastructure.Concurrency;
using EquipmentReservation.Infrastructure.Inventory;
using EquipmentReservation.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var inventory = new InMemoryInventoryModule();
var defaultEquipmentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
inventory.Seed(defaultEquipmentId, available: 10);

builder.Services.AddSingleton<IInventoryModule>(inventory);
builder.Services.AddSingleton<IInventoryReadModel>(inventory);
builder.Services.AddSingleton<IReservationRepository, InMemoryReservationRepository>();
builder.Services.AddSingleton<IReservationRequestLock, InMemoryReservationRequestLock>();
builder.Services.AddScoped<CreateReservationHandler>();
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)));

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "Equipment Reservation teaching example",
    endpoints = new[]
    {
        "GET /health",
        "GET /inventory/{equipmentId}",
        "POST /reservations"
    },
    demoEquipmentId = defaultEquipmentId
}));

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/inventory/{equipmentId:guid}", async (
    Guid equipmentId,
    IInventoryReadModel inventoryReadModel,
    CancellationToken cancellationToken) =>
{
    var available = await inventoryReadModel.GetAvailableAsync(equipmentId, cancellationToken);

    return available is null
        ? Results.NotFound(new { error = "EquipmentNotFound", equipmentId })
        : Results.Ok(new { equipmentId, available });
});

app.MapPost("/reservations", async (
    CreateReservationRequest request,
    CreateReservationHandler handler,
    CancellationToken cancellationToken) =>
{
    var command = new CreateReservationCommand(
        request.RequestId,
        request.EquipmentId,
        request.StudentId,
        request.Quantity);

    var result = await handler.HandleAsync(command, cancellationToken);

    return result.Outcome switch
    {
        CreateReservationOutcome.Confirmed => Results.Ok(result),
        CreateReservationOutcome.Rejected => Results.Conflict(result),
        _ => throw new InvalidOperationException("Unsupported reservation outcome.")
    };
});

app.Run();
