using Appointments.Api.Extensions;
using Appointments.Application;
using Appointments.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddOpenApi();
builder.Services.AddEndpoints();

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
app.RegisterEndpoints();

app.Run();