using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Organizer;


//TODO: I am changinng the update Status to be a method in the Event class

public class EventFactory {
    private Event _event;

    private EventFactory() { }

    public static EventFactory Init() {
        var factory = new EventFactory();
        factory._event = Event.Create(null).Payload;
        return factory;
    }

    public static EventFactory Init(OrganizerId oid) {
        var factory = new EventFactory();
        factory._event = Event.Create(null).Payload;
        // TODO TALK WITH TROELSfactory._event = Event.Create(oid).Payload;
        return factory;
    }


    public EventFactory WithStatus(EventStatus status) {
        _event.Status = status; // Ensure this is allowed; might need internal or public setter.
        return this;
    }

    public EventFactory WithMaxNumberOfGuests(int maxNumberOfGuests) {
        _event.MaxNumberOfGuests = maxNumberOfGuests;
        return this;
    }

    public EventFactory WithVisibility(EventVisibility visibility) {
        _event.Visibility = visibility;
        return this;
    }

    // Add more methods for other event properties as needed.

    public Event Build() {
        // Here, you could also validate the constructed event object.
        return _event;
    }

    public EventFactory WithValidTimeInFuture() {
        // Create the start DateTime by combining the date and time
        var startDate = DateTime.Today.AddDays(1);
        var startTime = new TimeSpan(8, 20, 0);
        var startDateTime = startDate.Add(startTime);

        // Create the end DateTime by combining the date and time
        var endDate = DateTime.Today.AddDays(1);
        var endTime = new TimeSpan(10, 20, 0);
        var endDateTime = endDate.Add(endTime);

        // Update the event with the new start and end DateTimes
        _event.UpdateTimeSpan(startDateTime, endDateTime);

        return this;
    }

    public EventFactory WithValidTimeInPast() {
        // Create the start DateTime by combining the date and time
        var startDate = DateTime.Today.AddDays(-1);
        var startTime = new TimeSpan(8, 20, 0);
        var startDateTime = startDate.Add(startTime);

        // Create the end DateTime by combining the date and time
        var endDate = DateTime.Today.AddDays(-1);
        var endTime = new TimeSpan(10, 20, 0);
        var endDateTime = endDate.Add(endTime);

        // Update the event with the new start and end DateTimes
        _event.UpdateTimeSpan(startDateTime, endDateTime);

        return this;
    }

    public EventFactory WithValidTitle() {
        _event.UpdateTitle("Valid Title");
        return this;
    }

    public EventFactory WithValidDescription() {
        _event.UpdateDescription("Valid Description");
        return this;
    }


    public EventFactory WithValidConfirmedAttendees(int confirmedAttendees) {
        var startEmailCount = 999999;
        for (var i = startEmailCount; i > startEmailCount - confirmedAttendees; i--) CreateAndRegisterGuest("John", "Doe", $"{i}@via.dk");

        return this;
    }

    private void CreateAndRegisterGuest(string firstName, string lastName, string email) {
        var guest = GuestFactory.Init(firstName, lastName, email).Build();
        guest.RegisterToEvent(_event);
    }
}