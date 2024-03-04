using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Entities.Invitation;

namespace UnitTests.Features.Guest.AcceptInvitation;

public class AcceptsInvitation {
    //ID: 14 – Guest accepts invitation

    //In order to join the event of which I was invited
    // As a guest
    // I want to accept the invitation
    //
    //

    //S1
    // Given an active event
    // And a registered guest
    // And the event has a pending invitation for the guest
    // And the number of participating (either invitation-accepts or participation-indications) guests is less than the maximum number of guests
    // When the guest accepts the invitation
    // Then the invitation is changed from pending to accepted
    //

    // Given an active event, and a registered guest, and the event has a pending invitation for the guest, and the number of participating (either invitation-accepts or participation-indications) guests is less than the maximum number of guests, when the guest accepts the invitation, then the invitation is changed from pending to accepted
    // ID:UC14.S1
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void GuestAcceptsInvitation_WithValidData_ShouldReturnSuccess(int maxNumberOfGuests) {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(maxNumberOfGuests)
            .WithValidConfirmedAttendees(maxNumberOfGuests - 1)
            .Build();

        @event.SendInvitation(guest);

        //Act
        var result = guest.AcceptInvitation(@event);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ParticipationStatus.Accepted, result.Payload.ParticipationStatus);
    }

    //F1
    // Given an existing event with ID
    // And a registered guest with ID
    // And the event has no invitation for the guest
    // When the guest accepts an invitation
    // Then the request is rejected, with a message explaining the guest is not invited to the event
    //

    // Given an existing event with ID, and a registered guest with ID, and the event has no invitation for the guest, when the guest accepts an invitation, then the request is rejected, with a message explaining the guest is not invited to the event
    // ID:UC14.F1
    [Fact]
    public void GuestAcceptsInvitation_WithNoInvitation_ShouldReturnFailure() {
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
        var result = guest.AcceptInvitation(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.InvitationNotFound, result.Error);
    }

    //F2
    // Given an existing event with ID
    // And a registered guest with ID
    // And the event has a pending invitation for the guest
    // And the number of participating (either invitation-accepts or participation-indications) guests has reached the maximum
    // When the guest accepts the invitation
    // Then the request is rejected, with a message explaining the event is full
    //

    // Given an existing event with ID, and a registered guest with ID, and the event has a pending invitation for the guest, and the number of participating (either invitation-accepts or participation-indications) guests has reached the maximum, when the guest accepts the invitation, then the request is rejected, with a message explaining the event is full
    // ID:UC14.F2
    [Fact]
    public void GuestAcceptsInvitation_WithMaxNumberOfGuests_ShouldReturnFailure() {
        //Arrange
        var guestOne = GuestFactory
            .Init("John", "Doe", "123456@via.dk")
            .Build();

        var guestTwo = GuestFactory
            .Init("Jane", "Doe", "654321@via.dk")
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(9)
            .Build();

        @event.SendInvitation(guestOne);
        @event.SendInvitation(guestTwo);
        guestOne.AcceptInvitation(@event);

        //Act
        var result = guestTwo.AcceptInvitation(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }
}