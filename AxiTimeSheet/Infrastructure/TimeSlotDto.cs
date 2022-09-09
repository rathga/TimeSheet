namespace AxiTimeSheet.Infrastructure;

public record TimeSlotDto(TimeOnly StartTime, TimeOnly? EndTime);

public static class TimeSlotExtensions
{
    public static TimeSlotDto ToDto(this TimeSlot timeSlot) => new(timeSlot.StartTime, timeSlot.HasFinished ? timeSlot.EndTime : null);
}
