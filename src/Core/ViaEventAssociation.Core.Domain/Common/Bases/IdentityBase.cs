using ViaEventAssociation.Core.Domain.Common.Bases;


public abstract class IdentityBase : ValueObject
{
    public string Value { get; }

    protected IdentityBase(string prefix) {
        Value = prefix + Guid.NewGuid();
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    public override string ToString() => Value;
}