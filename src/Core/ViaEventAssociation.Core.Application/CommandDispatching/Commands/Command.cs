namespace ViaEventAssociation.Core.Application.Features.Commands;

public abstract class Command<TId> {
    public Command(TId id) {
        Id = id;
    }

    public TId Id { get; }
}