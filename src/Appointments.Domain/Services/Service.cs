using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Services;

public sealed class Service : Entity, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }
    public TimeSpan Duration { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private Service() {}

    private Service(Guid id, string name, decimal price, TimeSpan duration, string? description, bool isActive) : base(id)
    {
        Name = name;
        Price = price;
        Duration = duration;
        Description = description;
        IsActive = isActive;
    }

    public static Result<Service> Create(string name, decimal price, TimeSpan duration, string? description = null, bool isActive = true)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result<Service>.Failure(ServiceErrors.NameIsRequired);

        if (price <= 0)
            return Result<Service>.Failure(ServiceErrors.PriceMustBeGreaterThanZero);

        if (duration <= TimeSpan.FromMinutes(5))
            return Result<Service>.Failure(ServiceErrors.DurationMustBeMoreThanFiveMinutes);

        if (duration > TimeSpan.FromDays(1))
            return Result<Service>.Failure(ServiceErrors.DurationMustBeLessThanOneDay);

        return Result<Service>.Success(new (Guid.NewGuid(), name, price, duration, description, isActive));
    }

    public Result UpdateInformation(string newName, string? newDescription)
    {
        if (string.IsNullOrWhiteSpace(newName))
            return Result.Failure(ServiceErrors.NameIsRequired);

        Name = newName;
        Description = newDescription;

        return Result.Success();
    }

    public Result AdjustPrice(decimal newPrice)
    {
        if (newPrice <= 0)
            return Result.Failure(ServiceErrors.PriceMustBeGreaterThanZero);

        Price = newPrice;

        return Result.Success();
    }

    public Result ChangeDuration(TimeSpan newDuration)
    {
        if (newDuration <= TimeSpan.FromMinutes(5))
            return Result.Failure(ServiceErrors.DurationMustBeMoreThanFiveMinutes);

        if (newDuration > TimeSpan.FromDays(1))
            return Result.Failure(ServiceErrors.DurationMustBeLessThanOneDay);

        Duration = newDuration;

        return Result.Success();
    }

    public void Activate() => IsActive = true;
    
    public void Deactivate() => IsActive = false;
}
