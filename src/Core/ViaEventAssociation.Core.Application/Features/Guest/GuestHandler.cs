using ViaEventAssociation.Core.Application.Features;
using ViaEventAssociation.Core.Application.Features.Commands;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

public abstract class GuestHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork) : CommandHandler<Guest, GuestId>(guestRepository, unitOfWork) {
    public async Task<Result> HandleAsync(Command<GuestId> command) {
        var result = await Repository.GetByIdAsync(command.Id);

        if (result.IsFailure)
            return result.Error;

        var guest = result?.Payload;

        if (guest is null)
            return Error.GuestIsNotFound;

        var action = await PerformAction(guest, command);
        if (action.IsFailure)
            return action.Error;

        await UnitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    protected abstract Task<Result> PerformAction(Guest guest, Command<GuestId> command);
}