using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public class Service
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; } = null;
    public Money Price { get; private set; } = null!;
    public DurationMinutes Duration { get; private set; } = null!;
    public bool IsActive { get; private set; }

    private Service() { }
    private Service(string name, string? description, Money price, DurationMinutes duration)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Duration = duration;
        IsActive = true;
    }

    public static Result<Service> Register(string name, string? description, Money price, DurationMinutes duration)
    {
        if (price == null)
            return Result<Service>.Failure(ServiceErrors.PriceIsRequired);

        if (duration == null)
            return Result<Service>.Failure(ServiceErrors.DurationIsRequired);

        if (string.IsNullOrWhiteSpace(name))
            return Result<Service>.Failure(ServiceErrors.NameCannotBeEmpty);

        return Result<Service>.Success(new Service(name, description, price, duration));
    }

    public Result UpdateInformation(string newName, string? newDescription, Money newPrice, DurationMinutes newDuration)
    {
        if (newDuration == null)
            return Result.Failure(ServiceErrors.DurationIsRequired);

        if (newPrice == null)
            return Result.Failure(ServiceErrors.PriceIsRequired);

        if (string.IsNullOrWhiteSpace(newName))
            return Result.Failure(ServiceErrors.NameCannotBeEmpty);

        Name = newName;
        Description = newDescription;
        Duration = newDuration;
        Price = newPrice;

        return Result.Success();
    }

    public void Deactivate() => IsActive = false;
}