using EquipmentReservation.Api.Composition;
using EquipmentReservation.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEquipmentReservation();

var app = builder.Build();

app.MapServiceEndpoints();
app.MapInventoryEndpoints();
app.MapReservationEndpoints();

app.Run();
