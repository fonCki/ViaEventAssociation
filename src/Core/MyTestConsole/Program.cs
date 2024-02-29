// See https://aka.ms/new-console-template for more information


using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation.Core.Domain.Agregates.Organizer;
using ViaEventAssociation.Core.Tools.OperationResult;

var result = Organizer.Create("adf", "g2@fma.com");
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

// var newEvent = organizator.CreateEvent().Value;
// Console.WriteLine(newEvent);
// Console.WriteLine(newEvent.Title);
// Console.WriteLine(newEvent.Id);
// Console.WriteLine(newEvent.MaxNumberOfGuests);
// Console.WriteLine(newEvent.Status);
// Console.WriteLine(newEvent.Visibility);
// Console.WriteLine(newEvent.Description);
// Console.WriteLine(newEvent.TimeSpan);

