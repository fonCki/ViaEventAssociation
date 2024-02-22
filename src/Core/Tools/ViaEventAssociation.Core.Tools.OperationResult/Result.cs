using ViaEventAssociation.Core.Tools.OperationResult;

public class Result {
    public bool IsSuccess { get; }
    public Error Error { get; }
    public bool IsFailure => !IsSuccess;


    protected Result(bool isSuccess, Error error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    // Factory method for failure
    public static Result Fail(Error error) {
        return new Result(false, error);
    }

    // Factory method for success
    public static Result Success() {
        return new Result(true, Error.NoError);
    }

    // Implicit operator for converting a string (error message) to a Result
    public static implicit operator Result(Error error) {
        return Fail(error);
    }

    // Implicit operator for converting a bool (success flag) to a Result
    public static implicit operator Result(bool successFlag) {
        return successFlag ? Success() : Fail(Error.InternalServerError);
    }

    public void OnSuccess(Action action) {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsSuccess) action.Invoke();
    }

    public void OnFailure(Action action) {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsFailure) action.Invoke();
    }
}

public class Result<T> : Result {
    public T Value { get; }

    private Result(bool isSuccess, T value, Error error) : base(isSuccess, error) {
        Value = value;
    }

    public new static Result<T> Fail(Error error) {
        return new Result<T>(false, default(T), error);
    }

    // Factory method for success with generic type
    public static Result<T> Success(T value) {
        return new Result<T>(true, value, Error.NoError);
    }

    public static implicit operator Result<T>(T value) {
        return Success(value);
    }

    public static implicit operator Result<T>(Error error) {
        return Fail(error);
    }

    public void OnSuccess(Action<T> action) {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsSuccess) action.Invoke(Value);
    }

    public void OnFailure(Action<Error> action) {
        if (action == null) throw new ArgumentNullException(nameof(action));
        if (IsFailure) action.Invoke(Error);
    }
}