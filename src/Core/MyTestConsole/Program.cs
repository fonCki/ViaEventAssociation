// See https://aka.ms/new-console-template for more information


using ViaEventAssociation.Core.Tools.OperationResult;

var result = Organizer.Create("The Canteen", "canteen@gmail.com");
var organizator = result.Payload;

if (result.IsFailure) {
    Error error = result.Error;
    while (error != null) {
        Console.WriteLine(error.Message);
        error = error.Next;
    }
}
else {
    organizator = result.Payload;


    Console.WriteLine(organizator);
    Console.WriteLine(organizator.OrganizerId);
    Console.WriteLine(organizator.OrganizerName.Value);
    Console.WriteLine(organizator.OrganizerEmail.Value);
}

var newEvent = organizator.CreateEvent().Payload;
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

newEvent.UpdateTitle("new Title").OnFailure(error => Console.WriteLine(error));


var r = newEvent.UpdateTitle("My New Title");

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

Console.WriteLine("************************************");
newEvent.UpdateDescription(" asdf asdkfh adsnvdsh fv;ashdnv djsfkbv c;kasjdfkasjdnfkjcdfsbc vad vajsdho;asdn vjdbfsncvjn dfsiv cduicnvfd vjdf in cvbdnsfionac sadsnvdsh fv;ashdnv djsfkbv c;kasjdfkasjdnfkjcdfsbc vad vajsdho;asdn vjdbfsncvjn dfsiv cduicnvfd vjdf in cvbdnsfionac sadsnvdsh fv;ashdnv djsfkbv c;kasjdfkasjdnfkjcdfsbc vad vajsdho;asdn vjdbfsncvjn dfsiv cduicnvfd vjdf in cvbdnsfionac s ").OnFailure(error => Console.WriteLine(error));


Console.WriteLine(newEvent.Description.Value);