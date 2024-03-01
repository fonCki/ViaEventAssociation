using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;

public class Event : AggregateRoot<EventId> {
    private static readonly int MAX_NUMBER_OF_GUESTS = 5;

    //TODO ask troels about this default title
    private static readonly string DEFAULT_TITLE = "Working Title";
    private Event(EventId id) : base(id) { }

    public Organizer Organizer { get; private set; }
    public EventTitle Title { get; private set; }
    public EventDescription Description { get; private set; }
    public DateTimeRange TimeSpan { get; private set; }
    public EventVisibility Visibility { get; private set; }
    public EventStatus Status { get; private set; }
    public int MaxNumberOfGuests { get; private set; }

    public static Result<Event> Create(Organizer organizer) {
        var newEvent = new Event(EventId.GenerateId().Payload) {
            Organizer = organizer,
            Title = EventTitle.Create(DEFAULT_TITLE).Payload,
            MaxNumberOfGuests = MAX_NUMBER_OF_GUESTS,
            Status = EventStatus.Canceled,
            Visibility = EventVisibility.Private,
            Description = EventDescription.InitEmpty().Payload
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
}