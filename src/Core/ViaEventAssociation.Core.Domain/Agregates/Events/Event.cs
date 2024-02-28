
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Domain.Common.Values;

public class Event : AggregateRoot<EventId> {

    private static readonly int MAX_NUMBER_OF_GUESTS = 5;
    //TODO ask troels about this default title
    private static readonly string DEFAULT_TITLE = "Working Title";

    public Organizer Organizer { get; private set;}
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTimeRange TimeSpan { get; private set; }
    public EventVisibility Visibility { get; private set; }
    public EventStatus Status { get; private set; }
    public int MaxNumberOfGuests { get; private set; }
    private Event(EventId id) : base(id) { }

    public static Result<Event> Create(Organizer organizer) {
        Event newEvent = new Event(EventId.GenerateId().Value) {
            Organizer = organizer,
            Title = DEFAULT_TITLE,
            MaxNumberOfGuests = MAX_NUMBER_OF_GUESTS,
            Status = EventStatus.Draft,
            Visibility = EventVisibility.Private,
            Description = string.Empty
        };
        return newEvent;
    }

    // public UpdateTitle(string title) {
    //     Title = title;
    // }
}