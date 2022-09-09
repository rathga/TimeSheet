using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.Domain.Commands;

public record SetTargetTimeCommand(TimeSpan TargetTime) : IRequest<Result>;

public class SetTargetTimeCommandHandler : IRequestHandler<SetTargetTimeCommand, Result>
{
    private readonly ITimeSheetRepository repo;

    public SetTargetTimeCommandHandler(ITimeSheetRepository repo)
    {
        this.repo = repo;
    }

    public Task<Result> Handle(SetTargetTimeCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(
            repo
                .Get()
                .CurrentDay.SetTargetTime(request.TargetTime))
                .Tap(() => repo.SaveChanges());
}
