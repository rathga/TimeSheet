using AxiTimeSheet.Domain.Commands;

namespace AxiTimeSheet.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateOnly GetDate() => DateOnly.FromDateTime(DateTime.Now);
    public TimeOnly GetTime() => TimeOnly.FromDateTime(DateTime.Now);
}