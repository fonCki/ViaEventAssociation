using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Agregates.Events;

public class EventTitle : ValueObject {
    private static readonly int MIN_TITLE_LENGTH = 3;
    private static readonly int MAX_TITLE_LENGTH = 75;

    private EventTitle(string title) {
        Value = title;
    }

    public string Value { get; }

    public static Result<EventTitle> Create(string title) {
        try {
            var validation = Validate(title);
            if (validation.IsFailure)
                return validation.Error;
            return new EventTitle(title);
        }
        catch (Exception exception) {
            return Error.FromException(exception);
        }
    }

    private static Result Validate(string title) {
        HashSet<Error> errors = new HashSet<Error>();

        if (title == null)
            return Error.NullString;

        if (string.IsNullOrWhiteSpace(title))
            errors.Add(Error.BlankString);

        if (title.Length < MIN_TITLE_LENGTH)
            errors.Add(Error.TooShortTitle(MIN_TITLE_LENGTH));

        if (title.Length > MAX_TITLE_LENGTH)
            errors.Add(Error.TooLongTitle(MAX_TITLE_LENGTH));
        if (errors.Any())
            return Error.Add(errors);

        return Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }
}