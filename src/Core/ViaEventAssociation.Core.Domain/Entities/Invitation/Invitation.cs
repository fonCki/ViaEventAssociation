using ViaEventAssociation.Core.Domain.Agregates.Guests;

namespace ViaEventAssociation.Core.Domain.Entities.Invitation;

public class Invitation : Participation {
    private Invitation(ParticipationId participationId, Event @event, Guest guest, ParticipationStatus participationStatus) : base(participationId, @event, guest, ParticipationType.Invitation, participationStatus) { }

    public static Result<Invitation> SendInvitation(Event @event, Guest guest) {
        var errors = new HashSet<Error>();

        ValidateParticipation(guest, @event)
            .OnFailure(error => errors.Add(error));

        var participationIdResult = ParticipationId.GenerateId()
            .OnFailure(error => errors.Add(error));

        if (errors.Any()) return Error.Add(errors);

        var participation = new Invitation(participationIdResult.Payload, @event, guest, ParticipationStatus.Pending);

        //Should I add the participation to the guest?
        guest.Participations.Add(participation);

        return participation;
    }
}