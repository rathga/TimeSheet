using System.Reflection;
using System.Text.Json;
using AxiTimeSheet.Domain;
using AxiTimeSheet.Domain.Commands;

namespace AxiTimeSheet.Infrastructure;

public class TimeSheetRepository : ITimeSheetRepository
{
    public const string CurrentDayFileName = "CurrentDay.json";

    private readonly TimeSpan defaultTargetTime;
    private readonly JsonSerializerOptions jsonOptions = new();

    private TimeSheet? timeSheet;
    private CurrentDay? currentDay;

    public TimeSheetRepository(TimeSpan defaultTargetTime)
    {
        this.defaultTargetTime = defaultTargetTime;
        jsonOptions.Converters.Add(new DateOnlyJsonConverter());
        jsonOptions.Converters.Add(new TimeOnlyJsonConverter());
    }

    public TimeSheet Get()
    {
        timeSheet ??= RetrieveOrCreateTimeSheet();
        currentDay = timeSheet.HasCurrentDay ? timeSheet.CurrentDay : null;
        return timeSheet;
    }

    public void SaveChanges()
    {
        if (timeSheet == null)
            throw new Exception("No timesheet to save");

        using (var stream = File.Open(CurrentDayFileName, FileMode.Truncate, FileAccess.Write))
        {
            JsonSerializer.Serialize(stream, timeSheet.CurrentDay.ToDto(), jsonOptions);
        }

        if (currentDay != null && currentDay != timeSheet?.CurrentDay)
        {
            using var stream =
                File.OpenWrite($"Day {currentDay.Date.Year}-{currentDay.Date.Month}-{currentDay.Date.Day}.json");
            JsonSerializer.Serialize(stream, currentDay.ToDto(), jsonOptions);
            currentDay = timeSheet?.CurrentDay;
        }
    }

    private TimeSheet RetrieveOrCreateTimeSheet()
    {
        if (!File.Exists(CurrentDayFileName))
            return new TimeSheet(defaultTargetTime);

        using var stream = File.OpenRead(CurrentDayFileName);
        var dayDto = JsonSerializer.Deserialize<DayDto>(stream, jsonOptions)!;

        var timeSheet = new TimeSheet(defaultTargetTime);
        typeof(TimeSheet)
            .SetPrivateField("currentDay", timeSheet, dayDto.ToEntity());

        return timeSheet;
    }
}

public static class ReflectionHelperExtensions
{
    public static void SetPrivateField(this Type type, string name, object instance, object? value) =>
        type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)!.SetValue(instance, value);

    public static TimeSlot ToEntity(this TimeSlotDto dto)
    {
        var timeSlotType = typeof(TimeSlot);
        var timeSlot = (TimeSlot) Activator.CreateInstance(
            timeSlotType, 
            BindingFlags.NonPublic | BindingFlags.Instance,
            null, 
            new object[] { dto.StartTime },
            null)!;
        timeSlotType.SetPrivateField("endTime", timeSlot, dto.EndTime);
        return timeSlot;
    }

    public static CurrentDay ToEntity(this DayDto dto)
    {
        var day = CurrentDay.New(dto.Date, dto.TargetTime);
        var currentDayType = typeof(CurrentDay);
        currentDayType.SetPrivateField("<StartingBank>k__BackingField", day, dto.StartingBank);
        currentDayType.SetPrivateField("timeSlots", day, dto.TimeSlots.Select(ToEntity).ToList());
        return day;
    }
}