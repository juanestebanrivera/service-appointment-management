using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Repositories;

public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Set<Appointment>().ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Appointment>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Set<Appointment>().AddAsync(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Appointment appointment)
    {
        _context.Set<Appointment>().Update(appointment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Appointment appointment)
    {
        _context.Set<Appointment>().Remove(appointment);
        await _context.SaveChangesAsync();
    }
}