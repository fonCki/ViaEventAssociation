using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Bases;
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

    protected static Result NoDuplicateParticipation(Guest guest, Event @event) {
        // Check if the user is already registered to the event
        if (guest.Participations.Any(p => p.Guest == guest && p.Event == @event))
            return Error.GuestAlreadyRegisteredToEvent;

        // Check if the user already requested to join the event
        if (@event.Participations.Any(p => p.Guest == guest))
            return Error.GuestAlreadyRequestedToJoinEvent;

        return Result.Ok;
    }

    public override string ToString() {
        return $"{nameof(Participation)}: {nameof(Id)}: {Id}, {nameof(Guest)}: {Guest}, {nameof(Event)}: {Event}, {nameof(ParticipationType)}: {ParticipationType}";
    }

    public Result CancelParticipation() {
        ParticipationStatus = ParticipationStatus.Cancelled;
        return Result.Ok;
    }
}