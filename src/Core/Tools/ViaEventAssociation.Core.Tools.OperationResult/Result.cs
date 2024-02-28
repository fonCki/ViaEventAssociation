using System.Runtime.InteropServices.JavaScript;
using ViaEventAssociation.Core.Tools.OperationResult;

public class Result {
    public bool IsSuccess { get; }
    public ErrorCollection Errors { get; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, ErrorCollection errors) {
        IsSuccess = isSuccess;
        Errors = errors;
    }


    // Overloaded factory method for failure with multiple errors
    public static Result Fail(ErrorCollection errors) {
        return new Result(false, errors);
    }

    // Factory method for success
    public static Result Success() {
        //TODO: Add a no error instance
        return new Result(true, null);
    }


    // Implicit operator for converting ErrorCollection to a Result
    public static implicit operator Result(ErrorCollection errors) {
        return Fail(errors);
    }

    // Implicit operator for converting a bool (success flag) to a Result
    public static implicit operator Result(bool successFlag) {
        // return successFlag ? Success() : Fail(ErrorCollection.FromError(Error.Unknown));
        return true;
    }

    public Result OnSuccess(Action action) {
        if (IsSuccess) {
            action?.Invoke();
        }
        return this; // Return current Result for chaining
    }

    //TODO Fix this
    // public Result OnFailure(Action<Error> action) {
    //     if (IsFailure) {
    //         action?.Invoke(action);
    //     }
    //     return this; // Return current Result for chaining
    // }
}

public class Result<T> : Result {
    public T Value { get; private set; }

    private Result(bool isSuccess, T value, ErrorCollection errors) : base(isSuccess, errors) {
        Value = value;
    }


    // Overloaded factory method for failure with multiple errors
    public static Result<T> Fail(ErrorCollection errors) {
        return new Result<T>(false, default(T), errors);
    }

    // Factory method for success with generic type
    public static Result<T> Success(T value) {
        return new Result<T>(true, value, null);
    }

    // Implicit operator for converting a value to a Result<T>
    public static implicit operator Result<T>(T value) {
        return Success(value);
    }


    // Implicit operator for converting ErrorCollection to a Result<T>
    public static implicit operator Result<T>(ErrorCollection errors) {
        return Fail(errors);
    }

    public Result<T> OnSuccess(Action<T> action) {
        if (IsSuccess) {
            action?.Invoke(this.Value);
        }
        return this; // Return current Result<T> for chaining
    }

    //TODO Fix this
    // public new Result<T> OnFailure(Action<Error> action) {
    //     if (IsFailure) {
    //         action?.Invoke(this.Error);
    //     }
    //     return this; // Return current Result<T> for chaining
    // }
}
