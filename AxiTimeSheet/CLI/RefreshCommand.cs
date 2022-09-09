using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

[Verb("Refresh", HelpText = "Refresh's the stats")]
public class RefreshCommand : ICommand { }

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<string>>
{
    public Task<Result<string>> Handle(RefreshCommand _, CancellationToken __) =>
        Task.FromResult(Result.Success("Refreshed"));
}
