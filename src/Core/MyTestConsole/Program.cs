// See https://aka.ms/new-console-template for more information

using ViaEventAssociation.Core.Domain.Agregates.Guests;

var result = Organizer.Create("The Canteen", "canteen@gmail.com");
var @event = Event.Create(result.Payload).Payload;


@event.UpdateTitle("The Canteen's 1st Anniversary");
@event.UpdateDescription("We are celebrating our 1st anniversary and we want to invite you to join us for a night of fun and celebration. We will have live music, food, and drinks. We hope to see you there!");
@event.UpdateTimeSpan(DateTime.Now.AddHours(2), DateTime.Now.AddHours(5));
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