using ViaEventAssociation.Core.Tools.OperationResult;

public class ErrorCollection {
    private List<Error> errors;

    // Make the constructor private so it cannot be called from outside
    private ErrorCollection() {
        this.errors = new List<Error>();
    }

    // Static method to initialize the ErrorCollection with the first error
    public static ErrorCollection AddFirst(Error error) {
        var collection = new ErrorCollection();
        collection.Add(error); // Use the private Add method internally
        return collection;
    }

    // Public instance method to add errors, allowing for fluent chaining
    public ErrorCollection Add(Error error) {
        this.errors.Add(error);
        return this; // Return the current instance for chaining
    }

    // Other necessary methods...
    public IEnumerable<Error> GetAll() {
        return errors.AsReadOnly();
    }

    public Error GetFirstOrDefault() {
        return errors.FirstOrDefault();
    }
}