using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Entities;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

public class Event : AggregateRoot<EventId> {
    private Event(EventId id) : base(id) { }
    public Organizer Organizer { get; private set; }
    public EventTitle Title { get; private set; }
    public EventDescription Description { get; private set; }
    public EventDateTime TimeSpan { get; set; } //TODO my setter is public for testing mode should I have a developer mode?
    internal EventVisibility Visibility { get; set; } //TODO my setter is public for testing mode should I have a developer mode?
    internal EventStatus Status { get; set; } //TODO ask troels about this this is public for testing mode
    internal int MaxNumberOfGuests { get; set; } //TODO my setter is public for testing mode should I have a developer mode?
    public HashSet<Participation> Participations { get; private set; }

    public int ConfirmedParticipations => Participations.Count(p => p.ParticipationStatus is ParticipationStatus.Accepted);

    public static Result<Event> Create(Organizer organizer) {
        var newEvent = new Event(EventId.GenerateId().Payload) {
            Organizer = organizer,
            Title = EventTitle.Create(CONST.DEFAULT_TITLE_EVENT).Payload,
            MaxNumberOfGuests = CONST.MIN_NUMBER_OF_GUESTS,
            Status = EventStatus.Draft,
            Visibility = EventVisibility.Private,
            Description = EventDescription.InitEmpty().Payload,
            Participations = new HashSet<Participation>()
        };
        return newEvent;
    }

    public Result UpdateTitle(string title) {
        // Use a list or hashset to collect errors since there can be multiple issues with the title.
        HashSet<Error> errors = new HashSet<Error>();

        var newTitle = EventTitle.Create(title)
            .OnFailure(error => errors.Add(error));

        if (Status is EventStatus.Active)
            errors.Add(Error.EventStatusIsActive);

        if (Status is EventStatus.Canceled)
            errors.Add(Error.EventStatusIsCanceled);

        // If there are any errors, return them
        if (errors.Any())
            return Result.Fail(Error.Add(errors)); // This assumes Error.Add can handle a HashSet<Error>

        // If there are no errors, update the title and return success
        Title = newTitle.Payload;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result UpdateDescription(string description) {
        // Use a list or hashset to collect errors since there can be multiple issues with the description.
        var errors = new HashSet<Error>();

        var newDescription = EventDescription.Create(description)
            .OnFailure(error => errors.Add(error));

        if (Status is EventStatus.Active)
            errors.Add(Error.EventStatusIsActive);

        if (Status is EventStatus.Canceled)
            errors.Add(Error.EventStatusIsCanceled);

        // If there are any errors, return them
        if (errors.Any())
            return Result.Fail(Error.Add(errors)); // This assumes Error.Add can handle a HashSet<Error>

        // If there are no errors, update the description and return success
        Description = newDescription.Payload;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result UpdateTimeSpan(DateTime start, DateTime end) {
        // Use a list or hashset to collect errors since there can be multiple issues with the time span.
        var errors = new HashSet<Error>();

        var newTimeSpan = EventDateTime.Create(start, end)
            .OnFailure(error => errors.Add(error));

        if (Status is EventStatus.Active)
            errors.Add(Error.EventStatusIsActive);

        if (Status is EventStatus.Canceled)
            errors.Add(Error.EventStatusIsCanceled);

        // If there are any errors, return them
        if (errors.Any())
            return Result.Fail(Error.Add(errors)); // This assumes Error.Add can handle a HashSet<Error>

        // If there are no errors, update the time span and return success
        TimeSpan = newTimeSpan.Payload;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result MakePublic() {
        if (Status is EventStatus.Canceled)
            return Result.Fail(Error.EventStatusIsCanceled);

        Visibility = EventVisibility.Public;
        return Result.Ok;
    }

    public Result MakePrivate() {
        if (Status is EventStatus.Active)
            return Result.Fail(Error.EventStatusIsActive);

        if (Status is EventStatus.Canceled)
            return Result.Fail(Error.EventStatusIsCanceled);

        Visibility = EventVisibility.Private;
        Status = EventStatus.Draft;
        return Result.Ok;
    }

    public Result SetMaxGuests(int maxGuests) {
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Active && maxGuests < MaxNumberOfGuests)
            errors.Add(Error.EventStatusIsActiveAndMaxGuestsReduced);

        if (Status is EventStatus.Canceled)
            errors.Add(Error.EventStatusIsCanceled);

        if (maxGuests < CONST.MIN_NUMBER_OF_GUESTS)
            errors.Add(Error.TooFewGuests(CONST.MIN_NUMBER_OF_GUESTS));

        if (maxGuests > CONST.MAX_NUMBER_OF_GUESTS)
            errors.Add(Error.TooManyGuests(CONST.MAX_NUMBER_OF_GUESTS));

        if (errors.Any())
            return Error.Add(errors);

        MaxNumberOfGuests = maxGuests;
        return Result.Ok;
    }

    public Result SetReady() {
        var errors = new HashSet<Error>();

        if (TimeSpan is null)
            errors.Add(Error.EventTimeSpanIsNotSet);

        if (Status is EventStatus.Canceled)
            errors.Add(Error.EventStatusIsCanceled);

        if (TimeSpan is not null && TimeSpan?.Start! < DateTime.Now)
            errors.Add(Error.EventTimeSpanIsInPast);

        if (Title.Value == CONST.DEFAULT_TITLE_EVENT)
            errors.Add(Error.EventTitleIsDefault);

        if (errors.Any())
            return Error.Add(errors);

        Status = EventStatus.Ready;
        return Result.Ok;
    }

    public Result Activate() {
        var errors = new HashSet<Error>();

        if (Status is EventStatus.Canceled)
            errors.Add(Error.EventStatusIsCanceled);

        if (TimeSpan is not null && TimeSpan?.Start! < DateTime.Now)
            errors.Add(Error.EventTimeSpanIsInPast);

        if (TimeSpan is null)
            errors.Add(Error.EventTimeSpanIsNotSet);

        if (errors.Any())
            return Error.Add(errors);

        Status = EventStatus.Active;
        return Result.Ok;
    }

    public Result<JoinRequest> RequestToJoin(JoinRequest joinRequest) {
        if (Visibility is EventVisibility.Private && string.IsNullOrEmpty(joinRequest.Reason))
            return Error.EventIsPrivate;

        if (Visibility is EventVisibility.Private && !isValidReason(joinRequest.Reason))
            return Error.JoinRequestReasonIsInvalid;

        Participations.Add(joinRequest);
        return joinRequest;
    }

    private bool isValidReason(string? joinRequestReason) {
        return joinRequestReason?.Length > 25;
    }

    public Result<Invitation> SendInvitation(Guest guest) {
        var result = Invitation.SendInvitation(this, guest)
            .OnSuccess(participation => Participations.Add(participation));


        if (result.IsFailure)
            return result.Error;

        return result.Payload;
    }

    public bool IsParticipating(Guest guest) {
        return Participations.Any(p => p.Guest == guest);
    }

    public bool IsInvitedButNotConfirmed(Guest guest) {
        return Participations.OfType<Invitation>().Any(p => p.Guest == guest && p.ParticipationStatus is ParticipationStatus.Pending);
    }

    public override string ToString() {
        return Title.Value;
    }
}