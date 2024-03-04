using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace ViaEventAssociation.Core.Domain.Entities;

public abstract class Participation : Entity<ParticipationId> {
    protected Participation(ParticipationId participationId, Event @event, Guest guest, ParticipationType participationType, ParticipationStatus participationStatus) : base(participationId) {
        Event = @event;
        Guest = guest;
        ParticipationType = participationType;
        ParticipationStatus = participationStatus;
    }

    public Guest Guest { get; private set; }
    public Event Event { get; private set; }
    public ParticipationType ParticipationType { get; private set; }
    public ParticipationStatus ParticipationStatus { get; protected set; }

    protected static Result ValidateParticipation(Guest guest, Event @event) {
        // Check if the event is full
        if (@event.ConfirmedParticipations >= @event.MaxNumberOfGuests)
            return Error.EventIsFull;

        // Check if the event is active
        if (@event.Status is not EventStatus.Active)
            return Error.EventStatusIsNotActive;

        // Check if the event is in the past
        if (DateTimeRange.isPast(@event.TimeSpan))
            return Error.EventTimeSpanIsInPast;

        // Check if the user is already registered to the event
        if (guest.IsPendingInEvent(@event) || guest.IsConfirmedInEvent(@event))
            return Error.GuestAlreadyParticipating;

        // Check if the user already requested to join the event
        if (@event.IsParticipating(guest) || @event.IsInvitedButNotConfirmed(guest))
            return Error.GuestAlreadyRequestedToJoinEvent;

        return Result.Ok;
    }

    public Result CancelParticipation() {
        if (DateTimeRange.isPast(Event.TimeSpan))
            return Error.EventIsPast;

        //Ad, remove participation from guest and from event
        //TODO troels: should I handle this from here?

        Guest.Participations.Remove(this);
        Event.Participations.Remove(this);
        return Result.Ok;
    }


    public Result AcceptParticipation() {
        var errors = new HashSet<Error>();

        if (Event.Status is not EventStatus.Active)
            errors.Add(Error.EventStatusIsNotActive);

        if (DateTimeRange.isPast(Event.TimeSpan))
            errors.Add(Error.EventTimeSpanIsInPast);

        if (ParticipationStatus != ParticipationStatus.Pending)
            errors.Add(Error.ParticipationStatusNotPending);

        if (Event.ConfirmedParticipations >= Event.MaxNumberOfGuests)
            errors.Add(Error.EventIsFull);

        if (errors.Any())
            return Error.Add(errors);

        ParticipationStatus = ParticipationStatus.Accepted;
        return Result.Ok;
    }


    public override string ToString() {
        return $"{nameof(Participation)}: {nameof(Id)}: {Id}, {nameof(Guest)}: {Guest}, {nameof(Event)}: {Event}, {nameof(ParticipationType)}: {ParticipationType}";
    }
}