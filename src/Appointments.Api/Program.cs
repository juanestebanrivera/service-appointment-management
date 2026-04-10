using Appointments.Api.Endpoints;
using Appointments.Application;
using Appointments.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddInfrastructure(connectionString);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
       options.SwaggerEndpoint("/openapi/v1.json", "Appointments API V1");
       options.RoutePrefix = string.Empty;
       options.DocumentTitle = "Appointments API Documentation";
    });
}

app.UseHttpsRedirection();

app.MapClientEndpoints();
app.MapServiceEndpoints();
app.MapAppointmentEndpoints();

app.Run();