using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace ViaEventAssociation.Core.Domain.Agregates.Guests;

public class Guest : AggregateRoot<GuestId> {
    private Guest(GuestId id, NameType firstName, NameType lastName, Email email) : base(id) {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Participations = new List<Participation>();
    }

    public NameType FirstName { get; }
    public NameType LastName { get; }
    public Email Email { get; }
    public List<Participation> Participations { get; }

    public static Result<Guest> Create(string firstName, string lastName, string email) {
        var errors = new HashSet<Error>();

        var FirstName = NameType.Create(firstName)
            .OnFailure(error => errors.Add(error));

        var LastName = NameType.Create(lastName)
            .OnFailure(error => errors.Add(error));

        var Mail = Email.Create(email)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        try {
            var guest = new Guest(GuestId.GenerateId().Payload, FirstName.Payload, LastName.Payload, Mail.Payload);
            return guest;
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    public Result<JoinRequest> RegisterToEvent(Event @event, string reason = null!) {
        var participationResult = JoinRequest.SubmitJoinRequest(@event, this, reason)
            .OnSuccess(participation => Participations.Add(participation));

        if (participationResult.IsFailure)
            return participationResult.Error;

        return participationResult.Payload;
    }

    public Result ReceiveInvitation(Invitation invitation) {
        Participations.Add(invitation);
        return Result.Ok;
    }

    public Result AcceptInvitation(Event @event) {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p => p.Event == @event);
        if (invitation is null)
            return Error.InvitationNotFound;

        var result = invitation.AcceptInvitation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result CancelParticipation(Event @event) {
        var participation = Participations.FirstOrDefault(p => p.Event == @event);
        if (participation is null)
            return Error.ParticipationNotFound;
        var result = participation.CancelParticipation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result RejectInvitation(Event @event) {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p => p.Event == @event);
        if (invitation is null)
            return Error.InvitationNotFound;
        var result = invitation.RejectInvitation();
        if (result.IsFailure)
            return result.Error;
        // Participations.Remove(invitation); TODO: Troels I am not sure if we should remove the invitation from the list
        return Result.Ok;
    }


    public override string ToString() {
        return $"{FirstName.Value} {LastName.Value}";
    }
}