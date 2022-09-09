using AxiTimeSheet.Domain;
using AxiTimeSheet.Domain.Commands;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

public class CommandLineInterface
{
    private readonly CommandParser commandParser;
    private readonly IMediator mediator;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ITimeSheetRepository timeSheetRepository;
    private readonly StatusRow timeWorkedStatusRow = new(2);
    private readonly StatusRow timeSlotStatusRow = new(3);
    private ErrorStatusRow commandStatusRow = null!;
    private TimeSheet timeSheet;

    public CommandLineInterface(
        CommandParser commandParser, 
        IMediator mediator, 
        IDateTimeProvider dateTimeProvider,
        ITimeSheetRepository timeSheetRepository)
    {
        this.commandParser = commandParser;
        this.mediator = mediator;
        this.dateTimeProvider = dateTimeProvider;
        this.timeSheetRepository = timeSheetRepository;
    }

    public async Task Run()
    {
        timeSheet = timeSheetRepository.Get();

        var shouldQuit = false;

        Console.WriteLine("Welcome to AxiTimeSheet"); // 0
        Console.WriteLine("Commands: newday [date] | start [time] | stop [time] | setTargetTime [time] | refresh | quit"); // 1
        Console.WriteLine(); // 2
        Console.WriteLine(); // 3
        Console.WriteLine(); // 4
        Console.WriteLine("---"); // 5

        commandStatusRow = new ErrorStatusRow(4, Console.ForegroundColor);

        do
        {
            timeWorkedStatusRow.SetStatus(GetTimeWorkedString());
            timeSlotStatusRow.SetStatus(GetTimeSlotStatusString());

            Console.SetCursorPosition(0, 6);
            var commandStr = Console.ReadLine() ?? "";

            Console.SetCursorPosition(0, 6);
            Console.Write(new string(' ', commandStr.Length));
   
            await commandParser.Parse(commandStr)
                .Tap(command => shouldQuit = command is QuitCommand)
                .Bind(async command => await mediator.Send(command))
                .Tap(result => commandStatusRow.SetStatus(result))
                .OnFailure(error => commandStatusRow.SetStatus(error, true));

            Console.SetCursorPosition(0, 6);

        } while (!shouldQuit);

    }

    private string GetTimeWorkedString()
    {
        if (timeSheet.HasCurrentDay)
            return $"Current Day: {timeSheet.CurrentDay.Date}" +
                   $" | Starting bank: {timeSheet.CurrentDay.StartingBank}" +
                   $" | Time worked so far today: {timeSheet.CurrentDay.GetTimeWorked(dateTimeProvider.GetTime())}" +
                   $" | Time remaining: {timeSheet.CurrentDay.GetTimeRemaining(dateTimeProvider.GetTime())}";

        return "No current day yet.  Use newday command.";
    }
    private string GetTimeSlotStatusString()
    {
        var lastTimeSlot = timeSheet.HasCurrentDay ? timeSheet.CurrentDay.TimeSlots.LastOrDefault() : null;

        if (lastTimeSlot is null)
            return "No timeslots today yet.";

        if (lastTimeSlot.HasFinished)
            return $"Last timeslot: {lastTimeSlot.StartTime}-{lastTimeSlot.EndTime}";

        return $"Current timeslot started: {lastTimeSlot.StartTime}";
    }

}