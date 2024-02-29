// See https://aka.ms/new-console-template for more information


using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation.Core.Domain.Agregates.Organizer;
using ViaEventAssociation.Core.Tools.OperationResult;

var result = Organizer.Create("The Canteen", "canteen@gmail.com");
Organizer organizator = result.Value;

if (result.IsFailure) {
    Error error = result.Error;
    while (error != null) {
        Console.WriteLine(error.Message);
        error = error.Next;
    }
}
else {
    organizator = result.Value;


    Console.WriteLine(organizator);
    Console.WriteLine(organizator.OrganizerId);
    Console.WriteLine(organizator.OrganizerName.Value);
    Console.WriteLine(organizator.OrganizerEmail.Value);
}

var newEvent = organizator.CreateEvent().Value;
Console.WriteLine("************************************");
Console.WriteLine(newEvent);
Console.WriteLine(newEvent.Title);
Console.WriteLine(newEvent.Id);
Console.WriteLine(newEvent.MaxNumberOfGuests);
Console.WriteLine(newEvent.Status);
Console.WriteLine(newEvent.Visibility);
Console.WriteLine(newEvent.Description);
Console.WriteLine(newEvent.TimeSpan);


//TODO: Talk with troels: Note (for session 2): Your ID type, e.g. Eventid, if it's not a plain Guid, but a wrapper, needs to be able to create an ID from a string of a Guid. Like: Eventid.FromString("...");
// WHY do I naed an ID? should not created automatically?

newEvent.UpdateTitle("df").OnFailure(error => Console.WriteLine(error));


var r = newEvent.UpdateTitle("df");

if (r.IsFailure) {
    Error error = r.Error;
    while (error != null) {
        Console.WriteLine(error.Message);
        error = error.Next;
    }
}
else {
    Console.WriteLine("Title updated!");
}
Console.WriteLine(newEvent.Title.Value);


