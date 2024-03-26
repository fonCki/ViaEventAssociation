using ViaEventAssociation.Core.Domain.Aggregates.Events;
using ViaEventAssociation.Core.Domain.Agregates.Events;

namespace UnitTests.Fakes;

public class InMemEventRepoStub : IEventRepository {
    // set up the in-memory database
    public readonly List<Event> _events = new();

    public Task<Result> AddAsync(Event aggregate) {
        _events.Add(aggregate);
        return Task.FromResult(Result.Success());
    }

    public Task<Result> UpdateAsync(Event aggregate) {
        // find the event in the list
        var existingEvent = _events.FirstOrDefault(e => e.Id == aggregate.Id);
        if (existingEvent == null) return Task.FromResult(Result.Fail(Error.EventIsNotFound));

        // update the event
        existingEvent = aggregate;
        return Task.FromResult(Result.Success());
    }

    public Task<Result> DeleteAsync(EventId id) {
        // find the event in the list
        var existingEvent = _events.FirstOrDefault(e => e.Id == id);
        if (existingEvent == null) return Task.FromResult(Result.Fail(Error.EventIsNotFound));

        // delete the event
        _events.Remove(existingEvent);
        return Task.FromResult(Result.Success());
    }

    public Task<Result<Event>> GetByIdAsync(EventId id) {
        // find the event in the list
        var existingEvent = _events.FirstOrDefault(e => e.Id == id);
        if (existingEvent == null) {
            var failureResult = Result.Fail(Error.EventIsNotFound) as Result<Event>;
            return Task.FromResult(failureResult);
        }

        return Task.FromResult(Result<Event>.Success(existingEvent));
    }

    public Task<Result<List<Event>>> GetAllAsync() {
        return Task.FromResult(Result<List<Event>>.Success(_events));
    }
}