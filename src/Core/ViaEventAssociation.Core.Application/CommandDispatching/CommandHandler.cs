using ViaEventAssociation.Core.Domain;
using ViaEventAssociation.Core.Domain.Common;
using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Application.Features;

//TODO: talk with Troels, instead of using a ICommandHandler I use a CommandHandler, which is a base class that implements the ICommandHandler interface. This way I can have a common constructor for all command handlers.
public abstract class CommandHandler<TCommand, TAgg, TId>
    where TAgg : AggregateRoot<TId>
    where TId : IdentityBase {
    protected readonly IRepository<TAgg, TId> Repository;
    protected readonly IUnitOfWork UnitOfWork;

    protected CommandHandler(IRepository<TAgg, TId> repository, IUnitOfWork unitOfWork) {
        Repository = repository;
        UnitOfWork = unitOfWork;
    }
}