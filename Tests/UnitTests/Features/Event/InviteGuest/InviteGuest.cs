using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Features.Event.InviteGuest;

public class InviteGuest {
    //Given an existing event with ID, and the event status is ready or active, and a registered guest with ID, when the creator invites a guest, then a pending guest invitation is registered on the event
    //ID:UC13.S1
    [Fact]
    public void CreatorInvitesGuest_WithValidEventAndGuest_ShouldReturnSuccess() {
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
        var result = @event.SendInvitation(guest);

        //Assert
        Assert.True(result.IsSuccess);
        //TODO Assert.Equal(ParticipationStatus.Pending, result.Payload.ParticipationStatus);
    }

    //Given an existing event with ID, and the event status is draft or cancelled, and a registered guest with ID, when the creator invites a guest, then the request is rejected with a message explaining guests can only be invited to the event, when the event is ready or active
    //ID:UC13.F1
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Canceled)]
    public void CreatorInvitesGuest_WithInvalidEventStatus_ShouldReturnFailure(EventStatus eventStatus) {
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
        var result = @event.SendInvitation(guest);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventStatusIsNotActive, result.Error);
    }

    //Given an existing event with ID, and the event status is active, and a registered guest with ID, and the maximum number of guests is already attending, counting participation-indications (UC11) and invitation-accepts (UC14), when the creator invites a guest, then the request is rejected with a message explaining you cannot invite guests if the event is full
    //ID:UC13.F2
    [Fact]
    public void CreatorInvitesGuest_WithMaxNumberOfGuests_ShouldReturnFailure() {
        //Arrange
        var guest = GuestFactory
            .InitWithDefaultsValues()
            .Build();

        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithMaxNumberOfGuests(10)
            .WithValidConfirmedAttendees(10)
            .Build();


        //Act
        var result = @event.SendInvitation(guest);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.EventIsFull, result.Error);
    }
}