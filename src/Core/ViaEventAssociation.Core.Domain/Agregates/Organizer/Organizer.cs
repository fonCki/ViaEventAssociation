using ViaEventAssociation.Core.Domain.Agregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Organizer;
using ViaEventAssociation.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;

public class Organizer {
    public OrganizerId OrganizerId { get; private set; }
    public OrganizerName OrganizerName { get; private set; }
    public Email OrganizerEmail { get; private set; }

    private Organizer(OrganizerId id, OrganizerName name, Email email) {
        OrganizerId = id;
        OrganizerName = name;
        OrganizerEmail = email;
    }

    public static Result<Organizer> Create(string name, string email) {
        ErrorCollection errors = new ErrorCollection();

        var idResult = OrganizerId.GenerateId();
        if (!idResult.IsSuccess) {
            errors.Add(idResult.Error);
        }

        var nameResult = OrganizerName.Create(name);
        if (!nameResult.IsSuccess) {
            errors.Add(nameResult.Error);
        }

        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess) {
            errors.Add(emailResult.Error);
        }

        if (errors.Errors.Any()) {
            return Result<Organizer>.Fail(errors);
        }

        return Result<Organizer>.Success(new Organizer(idResult.Value, nameResult.Value, emailResult.Value));
    }

    public Result<Event> CreateEvent() {
        var eventResult = Event.Create(this);
        return eventResult.IsSuccess ? Result<Event>.Success(eventResult.Value) : Result<Event>.Fail(eventResult.Error);
    }
}