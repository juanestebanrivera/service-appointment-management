namespace Appointments.Domain.SharedKernel;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; private init; }

    protected Entity() { }
    protected Entity(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("ID cannot be empty.", nameof(id));

        Id = id;
    }

    public bool Equals(Entity? entity)
    {
        if (entity is null)
            return false;

        if (ReferenceEquals(this, entity))
            return true;

        if (entity.GetType() != GetType())
            return false;

        return Id == entity.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Entity entity)
            return Equals(entity);

        return false;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}