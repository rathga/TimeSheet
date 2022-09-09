using CSharpFunctionalExtensions;
using static CSharpFunctionalExtensions.Result;

namespace AxiTimeSheet.Domain;

public class CurrentDay
{
    private readonly List<TimeSlot> timeSlots = new();

    private bool finished;

    public DateOnly Date { get; }
    public TimeSpan TargetTime { get; private set; }
    public TimeSpan StartingBank { get; }

    public IReadOnlyList<TimeSlot> TimeSlots => timeSlots;

    public TimeSpan GetTimeWorked(TimeOnly currentTime) => 
        timeSlots.Aggregate(TimeSpan.Zero, (t, slot) => t + ((slot.HasFinished ? slot.EndTime : currentTime) - slot.StartTime));

    public TimeSpan GetTimeRemaining(TimeOnly currentTime) => -StartingBank - GetTimeWorked(currentTime) + TargetTime;

    internal static CurrentDay New(DateOnly date, TimeSpan targetTime) =>
        new(date, targetTime, TimeSpan.Zero);

    private CurrentDay(DateOnly date, TimeSpan targetTime, TimeSpan startingBank)
    {
        Date = date;
        TargetTime = targetTime;
        StartingBank = startingBank;
    }

    public Result<TimeSlot> StartTimeSlot(TimeOnly startTime) =>
        AssertNotFinished()
            .Ensure(() => timeSlots.Count == 0 || timeSlots.Last().HasFinished, "Cannot start a new time slot whilst another timeslot is active.")
            .Ensure(() => timeSlots.Count == 0 || timeSlots.Last().EndTime <= startTime, $"Start time must be after the previous slot's endtime of {timeSlots.LastOrDefault()?.EndTime}")
            .Map(() => TimeSlot.Start(startTime))
            .Tap(timeSlot => timeSlots.Add(timeSlot));

    public Result<TimeSlot> StopTimeSlot(TimeOnly stopTime) =>
        AssertNotFinished()
            .Ensure(() => timeSlots.Count > 0, "No time slots exist")
            .Map(() => timeSlots.Last())
            .Bind(timeSlot => timeSlot.Stop(stopTime).Map(() => timeSlot));

    public Result SetTargetTime(TimeSpan targetTime) =>
        AssertNotFinished()
            .Ensure(() => targetTime.Ticks >= 0, "Target Time must be positive.")
            .Ensure(() => targetTime.TotalDays <= 1, "Target time cannot exceed 24 hours")
            .Tap(() => TargetTime = targetTime);

    internal Result<CurrentDay> MoveToNewDay(DateOnly date, TimeSpan targetTime) =>
        AssertNotFinished()
            .Ensure(() => timeSlots.Count == 0 || timeSlots.Last().HasFinished, "Current day has a time slot active")
            .Ensure(() => date > Date, "Date must be after current day's date")
            .Map(() => new CurrentDay(date, targetTime, -GetTimeRemaining(TimeOnly.MinValue)))
            .Tap(() => finished = true);

    private Result AssertNotFinished() => finished ? throw new Exception("Day is finished") : Success();

}

