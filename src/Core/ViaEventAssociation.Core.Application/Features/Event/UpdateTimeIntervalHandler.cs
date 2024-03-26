using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class UpdateTimeIntervalHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : CommandHandler<UpdateTimeIntervalCommand, global::Event, EventId>(eventRepository, unitOfWork) {
    public override async Task<Result> HandleAsync(UpdateTimeIntervalCommand command) {
        var @event = await Repository.GetByIdAsync(command.Id);

        if (@event is null)
            return Error.EventIsNotFound;

        var action = @event.Payload.UpdateTimeSpan(command.TimeInterval);

        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}