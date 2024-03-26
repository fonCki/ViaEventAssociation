namespace ViaEventAssociation.Core.Application.Features.Commands;

public class Command<Tid> {
    public Command(Tid id) {
        Id = id;
    }

    public Tid Id { get; }
}