using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class UpdateDescriptionHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : CommandHandler<UpdateDescriptionCommand, global::Event, EventId>(eventRepository, unitOfWork) {
    public override async Task<Result> HandleAsync(UpdateDescriptionCommand command) {
        var @event = await Repository.GetByIdAsync(command.Id);

        if (@event is null)
            return Error.EventIsNotFound;

        var action = @event.Payload.UpdateDescription(command.Description);

        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}