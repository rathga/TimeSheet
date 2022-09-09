using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

[Verb("Quit", HelpText = "Exits the program")]
public class QuitCommand : ICommand { }

public class QuitCommandHandler : IRequestHandler<QuitCommand, Result<string>>
{
    public Task<Result<string>> Handle(QuitCommand _, CancellationToken __) => 
        Task.FromResult(Result.Success("bye bye!"));
}