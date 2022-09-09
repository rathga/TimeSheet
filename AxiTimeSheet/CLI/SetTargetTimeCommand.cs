using AutoMapper;
using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

[Verb("SetTargetTime", HelpText = "Sets target time to work on current day")]
public class SetTargetTimeCommand : IDomainCommand<Domain.Commands.SetTargetTimeCommand, Result>
{
    [Value(0)] public TimeSpan TargetTime { get; set; }
}

public class SetTargetTimeHandler : DomainCommandHandler<SetTargetTimeCommand, Domain.Commands.SetTargetTimeCommand, Result>
{
    public SetTargetTimeHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    public override Result<string> MapResponse(SetTargetTimeCommand command, Result result) =>
        result.Map(() => $"Target time set to {command.TargetTime}");

}
