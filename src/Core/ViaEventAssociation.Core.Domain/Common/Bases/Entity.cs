namespace ViaEventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<TId> where TId : ValueObject
{
    public TId Id { get; private set; }

    protected Entity(TId id) {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (Entity<TId>)obj;
        return Id.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
