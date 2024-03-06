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
        var errors = new HashSet<Error>();

        if (IsPendingInEvent(@event).Payload || IsConfirmedInEvent(@event).Payload)
            errors.Add(Error.GuestAlreadyParticipating);

        var participationResult = JoinRequest.SendJoinRequest(@event, this, reason)
            .OnSuccess(participation => Participations.Add(participation));

        if (participationResult.IsFailure)
            errors.Add(participationResult.Error);

        if (errors.Any())
            return Error.Add(errors);

        return participationResult.Payload;
    }

    public Result Serve(Invitation invitation) {
        // Check if the user is already registered to the event
        var participation = Participations.FirstOrDefault(p => p.Event == invitation.Event && p.ParticipationStatus == ParticipationStatus.Accepted);

        if (participation is null) {
            Participations.Add(invitation);
            return Result.Ok;
        }

        if (participation.ParticipationStatus == ParticipationStatus.Accepted) return Result.Fail(Error.GuestAlreadyParticipating);

        if (participation is JoinRequest && participation.ParticipationStatus == ParticipationStatus.Pending) return Result.Fail(Error.GuestAlreadyRequestedToJoinEvent);

        if (participation is Invitation && participation.ParticipationStatus == ParticipationStatus.Pending) return Result.Fail(Error.GuestAlreadyInvited);

        return Error.GuestAlreadyParticipating;
    }

    public Result AcceptInvitation(Event @event) {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending);
        if (invitation is null)
            return Error.InvitationPendingNotFound;

        var result = invitation.AcceptsInvitation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result DeclineInvitation(Event @event) {
        var invitation = Participations.OfType<Invitation>().FirstOrDefault(p =>
            (p.Event == @event &&
             p.ParticipationStatus == ParticipationStatus.Pending) ||
            p.ParticipationStatus == ParticipationStatus.Accepted);

        if (invitation is null)
            return Error.InvitationPendingOrAcceptedNotFound;

        var result = invitation.DeclineInvitation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    //TODO improve this method
    public Result CancelParticipation(Event @event) {
        var participation = Participations.FirstOrDefault(p => p.Event == @event && p.ParticipationStatus != ParticipationStatus.Canceled);
        if (participation is null)
            return Result.Ok; //I could return an error that the user is not found. is better than do not act TROELS
        var result = participation.CancelParticipation();
        if (result.IsFailure)
            return result.Error;
        return Result.Ok;
    }

    public Result<bool> IsConfirmedInEvent(Event @event) {
        return Participations.Any(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Accepted);
    }

    public Result<bool> IsPendingInEvent(Event @event) {
        return Participations.Any(p => p.Event == @event && p.ParticipationStatus == ParticipationStatus.Pending);
    }

    public override string ToString() {
        return $"{FirstName} {LastName}";
    }
}