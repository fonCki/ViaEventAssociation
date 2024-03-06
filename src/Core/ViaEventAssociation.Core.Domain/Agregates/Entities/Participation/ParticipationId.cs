namespace ViaEventAssociation.Core.Domain.Entities;

public class ParticipationId : IdentityBase {
    private ParticipationId(string prefix) : base(prefix) { }

    public static Result<ParticipationId> GenerateId() {
        try {
            return new ParticipationId("PID");
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}