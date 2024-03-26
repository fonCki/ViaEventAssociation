using ViaEventAssociation.Core.Application.Features.Commands.Event;
using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace ViaEventAssociation.Core.Application.Features.Event;

internal class CreateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) : CommandHandler<CreateEventCommand, global::Event, EventId>(eventRepository, unitOfWork) {
    // private readonly IEventRepository _eventRepository;
    // private readonly IUnitOfWork _unitOfWork;

    // public CreateEventHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork) {
    //     _eventRepository = eventRepository;
    //     _unitOfWork = unitOfWork;
    // }

    public override async Task<Result> HandleAsync(CreateEventCommand command) {
        global::Event.Create(Organizer.Create("test", "test@test.com:").Payload);
        // var result = await _eventRepository.AddAsync(@event);
        // if (result.IsFailure) {
        // return Result.Failure(result.Error);
        // }

        // await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}