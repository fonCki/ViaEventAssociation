using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Agregates.Locations;

public class LocationId : IdentityBase
{
    private LocationId(string prefix) : base(prefix) { }

    public static Result<LocationId> GenerateId() {
        try {
            return new LocationId("LID");
        } catch (Exception exception) {
            return Result<LocationId>.Fail(Error.FromException(exception));
        }
        return new LocationId("LID");
    }
}