using ViaEventAssociation.Core.Domain.Common.Bases;

namespace ViaEventAssociation.Core.Domain.Common.Values;

public abstract class DateTimeRange : ValueObject {
    protected DateTimeRange(DateTime start, DateTime end) {
        Start = start;
        End = end;
    }

    public DateTime Start { get; }
    public DateTime End { get; }

    protected override IEnumerable<object> GetEqualityComponents() {
        yield return Start;
        yield return End;
    }

    public static bool isPast(DateTimeRange dateTimeRange) {
        return dateTimeRange.End < DateTime.Now;
    }

    public static bool isFuture(DateTimeRange dateTimeRange) {
        return dateTimeRange.Start > DateTime.Now;
    }

    public override string ToString() => $"Start: {Start}, End: {End}";
}