using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.Values;

public class Email: ValueObject {
    public string Value { get; private set; }

    private Email(string value) {
        Value = value;
    }

    public static Result<Email> Create(string email) {
        var validation = Validate(email);
        if (validation.IsSuccess) {
            return new Email(email);
        }
        return validation.Errors;
    }


    private static Result Validate(string email) {

        var errors = ErrorCollection.AddFirst(Error.InvalidEmail).Add(Error.BlankString).
        // ErrorCollection errors = null; // Initialize as null to indicate no errors yet
        //
        // // Check for blank email
        // if (string.IsNullOrWhiteSpace(email)) {
        //     var error = Error.BlankString; // Use your specific Error instance for blank string
        //     errors = errors == null ? ErrorCollection.AddFirst(error) : errors.Add(Error.Forbidden);
        // }
        //
        // // Check if email matches regular expression for email validation
        // if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) {
        //     var error = Error.InvalidEmail; // Use your specific Error instance for invalid email
        //     errors = errors == null ? ErrorCollection.AddFirst(error) : errors.Add(error);
        // }
        //
        // // Determine whether to return a single error or a collection
        // if (errors != null) {
        //     // There are errors, return them
        //     // If there's only one, GetFirstOrDefault will effectively return it
        //     return Result.Fail(errors.GetFirstOrDefault() ?? errors); // if GetFirstOrDefault() is null, errors itself is returned
        // }
        //
        // // If no errors, validation is successful
        // return Result.Success();
    }



    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}