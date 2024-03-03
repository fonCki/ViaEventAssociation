namespace ViaEventAssociation.Core.Domain.Agregates.Organizer;

public class OrganizerId : IdentityBase {
    private OrganizerId(string prefix) : base(prefix) { }

    public static Result<OrganizerId> GenerateId() {
        try {
            return new OrganizerId("OID");
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}