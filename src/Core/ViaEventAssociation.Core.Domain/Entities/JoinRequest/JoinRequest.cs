using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

public class JoinRequest : Participation {
    private JoinRequest(ParticipationId participationId, Event @event, Guest guest, string? reason, ParticipationStatus participationStatus) : base(participationId, @event, guest, ParticipationType.JoinRequest, participationStatus) {
        Reason = reason;
    }

    public string? Reason { get; private set; }

    public static Result<JoinRequest> SubmitJoinRequest(Event @event, Guest guest, string reason = null) {
        var errors = new HashSet<Error>();

        ValidateParticipation(guest, @event)
            .OnFailure(error => errors.Add(error));

        var participationIdResult = ParticipationId.GenerateId()
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        var participation = new JoinRequest(participationIdResult.Payload, @event, guest, reason, ParticipationStatus.Pending);
        var confirmParticipation = @event.ConfirmParticipation(participation);

        if (confirmParticipation.IsFailure)
            return confirmParticipation.Error;

        //update the ParticipationStatus in the participation
        participation.ParticipationStatus = ParticipationStatus.Accepted;

        return participation;
    }
}