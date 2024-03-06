// See https://aka.ms/new-console-template for more information

using ViaEventAssociation.Core.Domain.Agregates.Guests;
using ViaEventAssociation.Core.Domain.Agregates.Locations;
using ViaEventAssociation.Core.Domain.Common.Values;

var result = Organizer.Create("The Canteen", "canteen@gmail.com");
var @event = Event.Create(result.Payload).Payload;


@event.UpdateTitle("The Canteen's 1st Anniversary");
@event.UpdateDescription("We are celebrating our 1st anniversary and we want to invite you to join us for a night of fun and celebration. We will have live music, food, and drinks. We hope to see you there!");
@event.UpdateTimeSpan(DateTime.Now.AddHours(2), DateTime.Now.AddHours(4));
@event.MakePublic();
@event.SetMaxGuests(10);
@event.Activate();


var guest = Guest.Create("John", "Doe", "JDH@via.dk").Payload;

// var resul = @event.SendInvitation(guest);

var res = @event.SendInvitation(guest);

Console.WriteLine(res.IsSuccess);
res.OnFailure(error => Console.WriteLine(error));

Print();

// guest.RejectInvitation(@event);
Print();
@event.SendInvitation(guest);
Print();
guest.AcceptInvitation(@event);
Print();


void Print() {
//for each guest in the HashMap print the name
    Console.WriteLine("***************************************************");
    Console.WriteLine("The confirmed participations are:");
    foreach (var participation in @event.Participations) Console.WriteLine(participation + " " + participation.ParticipationStatus);


    Console.WriteLine("***************************************************");
    Console.WriteLine("The guest's participations are:");
    foreach (var participation in guest.Participations) Console.WriteLine(participation + " " + participation.ParticipationStatus);
}

var location = Location.Create().Payload;
location.UpdateName("The Canteen");
location.setsAvailableTimeSpan(DateTimeRange.Create(DateTime.Now.AddDays(-10), DateTime.Now.AddHours(10)).Payload);
location.SetMaxNumberOfGuests(40);

location.AddEvent(@event).OnFailure(error => Console.WriteLine(error));

location.setsAvailableTimeSpan(DateTimeRange.Create(DateTime.Now.AddHours(-10), DateTime.Now.AddHours(17)).Payload).OnFailure(error => Console.WriteLine(error));

var event2 = Event.Create(result.Payload).Payload;
event2.UpdateTitle("The Canteen's 2nd Anniversary");
event2.UpdateDescription("We are celebrating our 2nd anniversary and we want to invite you to join us for a night of fun and celebration. We will have live music, food, and drinks. We hope to see you there!");
event2.UpdateTimeSpan(DateTime.Now.AddHours(5), DateTime.Now.AddHours(6));
event2.MakePublic();
event2.Activate();
location.AddEvent(event2).OnFailure(error => Console.WriteLine(error));

Console.WriteLine(location);