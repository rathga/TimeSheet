using AxiTimeSheet.Domain;

namespace AxiTimeSheet.Infrastructure;

public record DayDto(
    DateOnly Date,
    TimeSpan TargetTime,
    TimeSpan StartingBank,
    ICollection<TimeSlotDto> TimeSlots);

public static class CurrentDayExtensions
{
    public static DayDto ToDto(this CurrentDay currentDay) => new(
        currentDay.Date,
        currentDay.TargetTime,
        currentDay.StartingBank,
        currentDay.TimeSlots.Select(TimeSlotExtensions.ToDto).ToArray());
}