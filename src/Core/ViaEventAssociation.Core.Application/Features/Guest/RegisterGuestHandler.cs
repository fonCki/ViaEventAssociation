using ViaEventAssociation.Core.Application.Features;
using ViaEventAssociation.Core.Application.Features.Commands;
using ViaEventAssociation.Core.Application.Features.Commands.Guest;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Agregates.Guests;

public class RegisterGuestHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork) : CommandHandler<Guest, GuestId>(guestRepository, unitOfWork) {
    public async Task<Result> HandleAsync(Command<GuestId> command) {
        var result = await Repository.GetByIdAsync(command.Id);

        // I need to check is the user already registered should I check by email or id?
        if (result.IsSuccess)
            return Error.GuestAlreadyRegistered;

        if (command is RegisterGuestCommand registerGuestCommand) {
            var guest = Guest.Create(registerGuestCommand.Id, registerGuestCommand.FirstName, registerGuestCommand.LastName, registerGuestCommand.Email);

            if (guest.IsFailure)
                return guest.Error;

            await Repository.AddAsync(guest.Payload); //TODO should I do this? troels??
            await UnitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        return Error.InvalidCommand;
    }
}