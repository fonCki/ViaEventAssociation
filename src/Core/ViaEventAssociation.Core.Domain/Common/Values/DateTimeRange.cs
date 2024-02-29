using ViaEventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult;

namespace ViaEventAssociation.Core.Domain.Common.Values;

public class DateTimeRange : ValueObject {
    public DateTime Start { get;}
    public DateTime End { get; }

    private DateTimeRange(DateTime start, DateTime end) {
        Start = start;
        End = end;
    }

    public static Result<DateTimeRange> Create(DateTime start, DateTime end) {
        var validation = Validate(start, end);
        if (validation.IsSuccess) {
            return new DateTimeRange(start, end);
        }

        return validation.Error;
    }

    private static Result Validate(DateTime start, DateTime end) {
        if (start == null || end == null)
            return Error.NullDateTime;
        return (end <= start) ? Error.InvalidDateTimeRange : Result.Ok;
    }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Start;
        yield return End;
    }

    public override string ToString() => $"Start: {Start}, End: {End}";
}