using CSharpFunctionalExtensions;
using static CSharpFunctionalExtensions.Result;

namespace AxiTimeSheet.Domain;

public class TimeSheet
{
    private readonly TimeSpan defaultTargetTime;
    private CurrentDay? currentDay;

    public CurrentDay CurrentDay => currentDay ?? throw new Exception("No current day");
    public bool HasCurrentDay => currentDay != null;
    public Result<CurrentDay> GetCurrentDay() =>
        FailureIf(!HasCurrentDay, "No current day.").Map(() => currentDay!);


    public TimeSheet(TimeSpan defaultTargetTime)
    {
        this.defaultTargetTime = defaultTargetTime;
    }

    public Result<CurrentDay> StartNewDay(DateOnly date) =>
         (currentDay?.MoveToNewDay(date, defaultTargetTime) 
         ?? Success(CurrentDay.New(date, defaultTargetTime)))
            .Tap(day => currentDay = day);

}
