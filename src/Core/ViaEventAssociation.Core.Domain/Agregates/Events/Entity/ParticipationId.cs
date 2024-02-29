using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Agregates.Events.Entity;

public class ParticipationId : IdentityBase
{
    private ParticipationId(string prefix) : base(prefix) { }

    public static Result<ParticipationId> GenerateId(){
        try {
            return new ParticipationId("PID");
        } catch (Exception exception) {
            return Error.FromException(exception);
        }
    }
}