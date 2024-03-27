using ViaEventAssociation.Core.Application.Features;
using ViaEventAssociation.Core.Application.Features.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

public abstract class EventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : CommandHandler<Event, EventId>(eventRepository, unitOfWork) {
    private Result<Event> @event;

    public async Task<Result> HandleAsync(Command<EventId> command) {
        var result = await Repository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return result.Error;

        var @event = result.Payload;

        if (@event is null)
            return Error.EventIsNotFound;

        var action = await PerformAction(@event, command);
        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    protected abstract Task<Result> PerformAction(Event eve, Command<EventId> command);
}