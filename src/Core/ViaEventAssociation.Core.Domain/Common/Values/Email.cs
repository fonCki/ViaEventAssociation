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
        return validation.IsSuccess ? new Email(email) : validation.Error;
    }

    private static Result Validate(string email) {
        HashSet<Error> errors = new HashSet<Error>();

        if (email == null)
            return Error.NullString;

        // Check if email matches regular expression for email validation
        if (string.IsNullOrWhiteSpace(email))
            errors.Add(Error.BlankString);

        // Check if email matches regular expression for email validation
        if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            errors.Add(Error.InvalidEmail);

        if (errors.Any())
            return Error.Add(errors);

        // If no errors, validation is successful
        return Result.Success();
    }



    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}