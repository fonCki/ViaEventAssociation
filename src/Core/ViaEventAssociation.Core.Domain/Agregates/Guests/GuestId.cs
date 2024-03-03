namespace ViaEventAssociation.Core.Domain.Agregates.Guests;

public class GuestId : IdentityBase {
    private GuestId(string prefix) : base(prefix) { }

    public static Result<GuestId> GenerateId() {
        try {
            return new GuestId("GID");
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}