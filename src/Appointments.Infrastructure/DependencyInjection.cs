using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Appointments.Domain.Users;
using Appointments.Infrastructure.Authentication;
using Appointments.Infrastructure.Persistence;
using Appointments.Infrastructure.Persistence.Appointments;
using Appointments.Infrastructure.Persistence.Clients;
using Appointments.Infrastructure.Persistence.Services;
using Appointments.Infrastructure.Persistence.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();


            return services;
        }
    }
}