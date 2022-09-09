using AutoMapper;
using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

[Verb("Start", HelpText = "Starts a new timeslot")]
public class StartCommand : IDomainCommand<Domain.Commands.StartCommand, Result<TimeSlot>>
{
    [Value(0)] public TimeOnly? StartTime { get; set; }
}

public class StartDomainCommandHandler : DomainCommandHandler<StartCommand, Domain.Commands.StartCommand, Result<TimeSlot>>
{
    public StartDomainCommandHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    public override Result<string> MapResponse(StartCommand command, Result<TimeSlot> result) =>
        result.Map(timeSlot => $"Started new timeslot at {timeSlot.StartTime}");
}
