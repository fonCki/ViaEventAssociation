using ViaEventAssociation.Core.Application.Features.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class SetReadyEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : EventHandler(eventRepository, unitOfWork) {
    protected override Task<Result> PerformAction(global::Event eve, Command<EventId> command) {
        if (command is SetReadyEventCommand setReadyEventCommand) return Task.FromResult(eve.SetReady());
        return Task.FromResult(Result.Fail(Error.InvalidCommand));
    }
}