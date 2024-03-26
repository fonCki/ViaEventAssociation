using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Commands.Event;

public class UpdateTimeIntervalCommand {
    private UpdateTimeIntervalCommand(EventId id, EventDateTime timeInterval) {
        (Id, TimeInterval) = (id, timeInterval);
    }

    public EventId Id { get; }
    public EventDateTime TimeInterval { get; }

    public static Result<UpdateTimeIntervalCommand> Create(string id, string start, string end) {
        var errors = new HashSet<Error>();

        var eventId = EventId.Create(id)
            .OnFailure(error => errors.Add(error));

        var eventTimeInterval = EventDateTime.CreateFromStrings(start, end)
            .OnFailure(error => errors.Add(error));

        if (errors.Any())
            return Error.Add(errors);

        return new UpdateTimeIntervalCommand(eventId.Payload, eventTimeInterval.Payload);
    }
}