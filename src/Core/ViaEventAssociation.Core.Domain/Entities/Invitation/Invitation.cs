using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Domain.Entities.Invitation;

public class Invitation : Participation {
    private Invitation(ParticipationId participationId, Event @event, Guest guest, ParticipationStatus participationStatus) : base(participationId, @event, guest, ParticipationType.Invitation, participationStatus) { }

    public static Result<Invitation> SendInvitation(Event @event, Guest guest) {
        var errors = new HashSet<Error>();

        NoDuplicateParticipation(guest, @event)
            .OnFailure(error => errors.Add(error));

        var participationIdResult = ParticipationId.GenerateId().OnFailure(error => errors.Add(error));

        if (errors.Any()) return Error.Add(errors);

        var participation = new Invitation(participationIdResult.Payload, @event, guest, ParticipationStatus.Pending);

        var result = guest.ReceiveInvitation(participation);

        if (result.IsFailure) return result.Error;

        return participation;
    }

    public Result AcceptInvitation() {
        if (ParticipationStatus != ParticipationStatus.Pending) return Error.InvitationStatusNotPending;

        ParticipationStatus = ParticipationStatus.Accepted;
        return Result.Ok;
    }

    public Result RejectInvitation() {
        if (ParticipationStatus != ParticipationStatus.Pending) return Error.InvitationStatusNotPending;

        ParticipationStatus = ParticipationStatus.Rejected;
        return Result.Ok;
    }
}