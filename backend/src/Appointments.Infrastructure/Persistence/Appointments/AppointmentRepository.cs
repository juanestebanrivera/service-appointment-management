using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.Appointments;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Appointments;

internal sealed class AppointmentRepository(ApplicationDbContext dbContext) : IAppointmentRepository, IQueryableRepository<Appointment>
{
    private readonly DbSet<Appointment> _appointments = dbContext.Set<Appointment>();

    public async Task<(IEnumerable<Appointment> Items, int TotalCount)> GetPagedAsync(PaginationParams pagination, string? searchQuery = null, CancellationToken cancellationToken = default)
    {
        var query = _appointments.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchQuery) &&
            Enum.TryParse<AppointmentStatus>(searchQuery, ignoreCase: true, out var status))
        {
            query = query.Where(a => a.Status == status);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(a => a.TimeRange.StartTime)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _appointments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> VerifyAvailabilityAsync(DateTimeOffset startTime, DateTimeOffset endTime, Guid? excludeAppointmentId = null, CancellationToken cancellationToken = default)
    {
        return !await _appointments.AnyAsync(a =>
            a.Id != excludeAppointmentId &&
            ((startTime >= a.TimeRange.StartTime && startTime < a.TimeRange.EndTime) ||
             (endTime > a.TimeRange.StartTime && endTime <= a.TimeRange.EndTime) ||
             (startTime <= a.TimeRange.StartTime && endTime >= a.TimeRange.EndTime)),
            cancellationToken);
    }

    public void Add(Appointment appointment)
    {
        _appointments.Add(appointment);
    }

    public void Update(Appointment appointment)
    {
        _appointments.Update(appointment);
    }
}