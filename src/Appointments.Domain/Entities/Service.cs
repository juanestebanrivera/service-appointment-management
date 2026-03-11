using Appointments.Domain.Common;
using Appointments.Domain.Exceptions;

namespace Appointments.Domain.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public TimeSpan EstimatedDuration { get; set; }
    public bool IsActive { get; set; }

    public Service(string name, decimal price, TimeSpan estimatedDuration)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainValidationException("Name is required", nameof(Name));

        if (price < 0)
            throw new DomainValidationException("Price must be higher than zero", nameof(Price));

        if (estimatedDuration.Hours > TimeSpan.HoursPerDay)
            throw new DomainValidationException("Estimated duration cannot be longer than one day", nameof(EstimatedDuration));

        Name = name;
        Price = price;
        EstimatedDuration = estimatedDuration;
        IsActive = true;
    }
}
