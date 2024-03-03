using ViaEventAssociation.Core.Domain.Agregates.Events;

public class CreateEvent {
    //TODO make no sense to give an ID to a new event


    // Create event with null id should produce draft status, and the maximum number of guests is 5
    // ID:UC1.S1
    [Fact]
    public void CreateEvent_WithNullId_ProduceDraftStatus_AndTheMaximumNumberOfGuestsIs5() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert
        Assert.Equal(EventStatus.Draft, @event.Status);
        Assert.Equal(5, @event.MaxNumberOfGuests);
    }

    // Create event with null id should produce title "Working Title"
    // ID:UC1.S2
    [Fact]
    public void CreateEvent_WithNullId_ProduceTitleWorkingTitle() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert
        Assert.Equal("Working Title", @event.Title.Value);
    }

    // Create event with null id should produce private visibility
    // ID:UC1.S3
    [Fact]
    public void CreateEvent_WithNullId_ProduceEmptyDescription() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert

        Assert.Equal(string.Empty, @event.Description.Value);
    }

    //S4
    // Given an Id
    // When creator selects to create an event
    // Then the event is created
    // And the visibility of the event is private

    [Fact]
    public void CreateEvent_WithNullId_ProducePrivateVisibility() {
        // Arrange
        Event @event;

        // Act
        @event = EventFactory.Init().Build();

        // Assert
        Assert.Equal(EventVisibility.Private, @event.Visibility);
    }
}