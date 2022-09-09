using AutoMapper;
using AxiTimeSheet.Domain;
using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

[Verb("NewDay", HelpText = "Starts a new day")]
public class NewDayCommand : IDomainCommand<Domain.Commands.NewDayCommand, Result<CurrentDay>>
{
    [Value(0)] public DateOnly? Date { get; set; }
}

public class NewDayCommandHandler : DomainCommandHandler<NewDayCommand, Domain.Commands.NewDayCommand, Result<CurrentDay>>
{
    public NewDayCommandHandler(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    public override Result<string> MapResponse(NewDayCommand command, Result<CurrentDay> result) =>
        result.Map(newDay => $"New day started: {newDay.Date}");

}
