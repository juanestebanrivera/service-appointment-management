using Appointments.Api.Endpoints;
using Appointments.Application;
using Appointments.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapClientEndpoints();
app.MapServiceEndpoints();
app.MapAppointmentEndpoints();

app.Run();