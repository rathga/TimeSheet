using AutoMapper;
using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

[Verb("Stop", HelpText = "Stops current timeslot")]
public class StopCommand : IDomainCommand<Domain.Commands.StopCommand, Result<TimeSlot>>
{
    [Value(0)] public TimeOnly? StopTime { get; set; }
}

public class StopCommandHandler : DomainCommandHandler<StopCommand, Domain.Commands.StopCommand, Result<TimeSlot>>
{
    public StopCommandHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    public override Result<string> MapResponse(StopCommand command, Result<TimeSlot> result) =>
        result.Map(timeSlot => $"Stopped current timeslot at {timeSlot.EndTime}");
}
