using ViaEventAssociation.Core.Domain.Agregates.Events;

public class CancelEventParticipation {
    //Given an existing event with ID, and a registered guest with ID, and the guest is currently marked as participating in the event, when the guest chooses to cancel their participation, then the event removes the participation of this guest
    //ID:UC12.S1
    [Fact]
    public void GuestCancelsParticipation_WithValidEventAndGuest_ShouldRemoveParticipation() {
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
        var result = guest.CancelParticipation(@event);

        //Assert
        Assert.True(result.IsSuccess);
        Assert.False(@event.IsParticipating(guest));
        Assert.False(guest.IsConfirmedInEvent(@event));
    }

    //Given an existing event with ID, and a registered guest with ID, and the guest is not marked as participating in the event, when the guest chooses to cancel their participation, then nothing changes
    //ID:UC12.S2
    [Fact]
    public void GuestCancelsParticipation_WithValidEventAndGuestNotParticipating_ShouldNotChangeParticipation() {
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
        var result = guest.CancelParticipation(@event);

        //Assert
        //TODO I make return an error that is the user is not found. is better than do not act TROELS
        Assert.True(result.IsFailure);
        Assert.Contains(Error.ParticipationNotFound, result.Error.GetAllErrors());
    }

    //Given an existing event with ID, and a registered guest with ID, and the guest is marked as participating in the event, and the event start time is in the past, when the guest chooses to cancel their participation, then the request is rejected, and a message explains you cannot cancel your participation of past or ongoing events
    //ID:UC12.F1
    //TODO Troels: this fucked up my setters
    [Fact]
    public void GuestCancelsParticipation_WithValidEventAndGuestParticipatingAndEventInPast_ShouldNotChangeParticipation() {
        //Arrange
        var @event = EventFactory.Init()
            .WithValidTimeInFuture()
            .WithMaxNumberOfGuests(5)
            .WithStatus(EventStatus.Active)
            .WithVisibility(EventVisibility.Public)
            .WithValidConfirmedAttendees(1)
            .Build();


        var guest = @event.Participations.First().Guest;
        @event.TimeSpan = EventDateTime.Create(DateTime.Now.AddDays(-1), DateTime.Now.AddHours(1).AddDays(-1)).Payload;

        //Act
        var result = guest.CancelParticipation(@event);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Contains(Error.EventIsPast, result.Error.GetAllErrors());
    }
}