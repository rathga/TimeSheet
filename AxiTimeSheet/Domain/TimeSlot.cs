using CSharpFunctionalExtensions;

namespace AxiTimeSheet;

public class TimeSlot
{
    private TimeOnly? endTime;
    public TimeOnly StartTime { get; }

    public TimeOnly EndTime => 
        endTime ?? throw new Exception($"{nameof(EndTime)} only available for finished timeslots.");

    public bool HasFinished => endTime.HasValue;

    internal static TimeSlot Start(TimeOnly startTime) => new(startTime);

    public Result Stop(TimeOnly endTime)
    {
        if (HasFinished)
            return Result.Failure("Time slot already finished");

        if (endTime <= StartTime) 
            return Result.Failure($"End time is before start time of {StartTime}");

        this.endTime = endTime;
        return Result.Success();
    }

    private TimeSlot(TimeOnly startTime)
    {
        StartTime = startTime;
    }

    public void Finish(TimeOnly endTime)
    {
        this.endTime = endTime;
    }


}

