using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class UpdateTitleHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : CommandHandler<UpdateTitleCommand, global::Event, EventId>(eventRepository, unitOfWork) {
    public override async Task<Result> HandleAsync(UpdateTitleCommand command) {
        var @event = await Repository.GetByIdAsync(command.Id); // Get the event by id

        if (@event is null)
            return Error.EventIsNotFound;

        var action = @event.Payload.UpdateTitle(command.Title); // Update the title of the event

        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}