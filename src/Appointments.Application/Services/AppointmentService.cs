using Appointments.Application.Dtos.Appointments;
using Appointments.Application.Interfaces.Repositories;
using Appointments.Application.Interfaces.Services;
using Appointments.Application.Mappings;
using Appointments.Domain.Entities;

namespace Appointments.Application.Services;

public class AppointmentService(IAppointmentRepository repository) : IAppointmentService
{
    private readonly IAppointmentRepository _repository = repository;

    public async Task<IEnumerable<AppointmentResponse>> GetAllAsync()
    {
        var appointments = await _repository.GetAllAsync();
        return appointments.Select(appointment => appointment.ToAppointmentResponse());
    }

    public async Task<AppointmentResponse?> GetByIdAsync(Guid id)
    {
        var appointment = await _repository.GetByIdAsync(id);
        return appointment?.ToAppointmentResponse();
    }

    public async Task<AppointmentResponse> CreateAsync(CreateAppointmentRequest request)
    {
        var appointment = new Appointment(request.ClientId, request.ServiceId);
        appointment.ScheduleService(request.ScheduledTime, request.Duration);

        await _repository.AddAsync(appointment);

        return appointment.ToAppointmentResponse();
    }

    public async Task<AppointmentResponse?> DeleteAsync(Guid id)
    {
        var appointment = await _repository.GetByIdAsync(id);

        if (appointment is null)
            return null;

        await _repository.DeleteAsync(id);

        return appointment.ToAppointmentResponse();
    }


    public async Task<AppointmentResponse?> Reschedule(Guid id, RescheduleAppointmentRequest request)
    {
        var appointment = await _repository.GetByIdAsync(id);

        if (appointment is null)
            return null;

        appointment.ScheduleService(request.ScheduledTime, request.Duration);
        await _repository.UpdateAsync(appointment);

        return appointment.ToAppointmentResponse();
    }

    public async Task<AppointmentResponse?> UpdateStatusAsync(Guid id, UpdateStatusAppointmentRequest request)
    {
        var appointment = await _repository.GetByIdAsync(id);

        if (appointment is null)
            return null;

        appointment.UpdateStatus(request.Status);
        await _repository.UpdateAsync(appointment);

        return appointment.ToAppointmentResponse();
    }
}