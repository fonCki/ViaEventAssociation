using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Agregates.Organizer;

public class OrganizerName: ValueObject {
    //TODO: Add a max length to the organizer name error multiple times
    private static readonly int MAX_LENGTH = 50;
    public string Value { get; }

    private OrganizerName(string value) {
        Value = value;
    }

    public static Result<OrganizerName> Create(string name) {
        var validation = Validate(name);
        if (validation.IsSuccess) {
            return new OrganizerName(name);
        }
        return Result<OrganizerName>.Fail(validation.Error);
    }

    private static Result Validate(string name) {
        if (string.IsNullOrWhiteSpace(name)) {
            return Error.InvalidOrganizerName;
        }
        return true;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}