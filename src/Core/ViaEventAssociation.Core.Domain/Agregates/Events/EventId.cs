using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Aggregates.Events;

public class EventId : IdentityBase {
    private EventId(string prefix) : base(prefix) {}

    public static Result<EventId> GenerateId() {
        try {
            return new EventId("EID");
        } catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}