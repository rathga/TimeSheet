namespace AxiTimeSheet.Domain.Commands;

public interface IDateTimeProvider
{
    DateOnly GetDate();
    TimeOnly GetTime();
}