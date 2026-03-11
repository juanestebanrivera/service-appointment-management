using Appointments.Application.Dtos.Appointments;

namespace Appointments.Application.Interfaces.Services;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentResponse>> GetAllAsync();
    Task<AppointmentResponse?> GetByIdAsync(Guid id);
    Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request);
    Task<AppointmentResponse?> DeleteAsync(Guid id);
    Task<AppointmentResponse?> Reschedule(Guid id, RescheduleAppointmentRequest request);
    Task<AppointmentResponse?> UpdateStatusAsync(Guid id, UpdateStatusAppointmentRequest request);
}