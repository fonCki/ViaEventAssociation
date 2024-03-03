using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Entities.Invitation;
using Xunit.Abstractions;

namespace UnitTests.Features.Guest.ParticipatePublicEvent;

public class GuestParticipatePublicEvent {
    private readonly ITestOutputHelper _testOutputHelper;

    public GuestParticipatePublicEvent(ITestOutputHelper testOutputHelper) {
        _testOutputHelper = testOutputHelper;
    }
    //ID: 11 – Guest participates public event
    //In order to indicate my intention to participate in an event
    // As a guest
    // I want to choose to participate in a public event
    //
    // Success scenari
    //

    //S1
    // Given an existing event with ID
    // And the event status is active
    // And the event is public
    // And a registered guest with ID
    // And the current number of registered guests is less than the maximum number of allowed guests
    // And the event has not yet started, i.e. before the start time
    // When the guest chooses to attend the public event
    // Then the event has registered that the guest intends to participate
    //

    // Given an existing event with ID, and the event status is active, and the event is public, and a registered guest with ID, and the current number of registered guests is less than the maximum number of allowed guests, and the event has not yet started, i.e. before the start time, when the guest chooses to attend the public event, then the event has registered that the guest intends to participate
    // ID:UC11.S1
    [Fact]
    public void GuestParticipatePublicEvent_WithValidData_ShouldReturnSuccess() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Accepted, result.Payload.ParticipationStatus);
    }

    //F1
    // Given an existing valid event with ID
    // And the event status is draft, ready, or cancelled
    // And a registered guest with ID
    // And the event is public
    // When guest chooses to participate in the event
    // Then the request is rejected, and a failure message is provided explaining that only active events can be joined
    //

    // Given an existing valid event with ID, and the event status is draft, ready, or cancelled, and a registered guest with ID, and the event is public, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that only active events can be joined
    // ID:UC11.F1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Canceled)]
    public void GuestParticipatePublicEvent_WithInvalidEventStatus_ShouldReturnFailure(EventStatus eventStatus) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(eventStatus)
            .WithVisibility(EventVisibility.Public)
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }


    //F2
    // Given an existing valid event with ID
    // And the event status is active
    // And a registered guest with ID
    // And the current number of registered guests (participants, invitees who have accepted, and accepted participate-requests) is equal to the maximum number of allowed guests
    // And the event is public
    // When guest chooses to participate in the event
    // Then the request is rejected, and a failure message is provided explaining that there is no more room
    //

    // Given an existing valid event with ID, and the event status is active, and a registered guest with ID, and the current number of registered guests (participants, invitees who have accepted, and accepted participate-requests) is equal to the maximum number of allowed guests, and the event is public, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that there is no more room
    // ID:UC11.F2
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    public void GuestParticipatePublicEvent_WithMaxNumberOfGuests_ShouldReturnFailure(int maxNumberOfGuests) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var newEvent = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .WithValidConfirmedAttendees(maxNumberOfGuests)
            .Build();

        //Act
        var result = guest.RegisterToEvent(newEvent);

        //Assert
        // _testOutputHelper.WriteLine(result.Payload.Event.Participations.Count.ToString());
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }


    //F3
    // Given an existing valid event with ID
    // And the event start time is in the past
    // When guest chooses to participate in the event
    // Then the request is rejected, and a failure message is provided explaining that only future events can be participated
    //

    // Given an existing valid event with ID, and the event start time is in the past, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that only future events can be participated
    // ID:UC11.F3
    [Fact]
    public void GuestParticipatePublicEvent_WithPastEvent_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInPast()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventTimeSpanIsInPast, result.Error);
    }

    //F4
    // Given an existing valid event with ID
    // And a registered guest with ID
    // And the event is private
    // When guest chooses to participate in the event
    // Then the request is rejected, and a failure message is provided explaining that only public events can be participated
    //

    // Given an existing valid event with ID, and a registered guest with ID, and the event is private, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that only public events can be participated
    // ID:UC11.F4
    [Fact]
    public void GuestParticipatePublicEvent_WithPrivateEvent_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Private)
            .Build();

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsPrivate, result.Error);
    }

    //F5
    // Given an existing valid event with ID
    // And a registered guest with ID
    // And the guest is already a participant at the event
    // When guest chooses to participate in the event
    // Then the request is rejected, and a failure message is provided explaining that a guest cannot take two slots at an event
    //

    // Given an existing valid event with ID, and a registered guest with ID, and the guest is already a participant at the event, when guest chooses to participate in the event, then the request is rejected, and a failure message is provided explaining that a guest cannot take two slots at an event
    // ID:UC11.F5
    [Fact]
    public void GuestParticipatePublicEvent_WithAlreadyParticipatingGuest_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .Build();

        guest.RegisterToEvent(@event);

        //Act
        var result = guest.RegisterToEvent(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.GuestAlreadyRegisteredToEvent, result.Error);
    }
}