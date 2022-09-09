using CommandLine;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.Domain.Commands;

public record StartCommand(TimeOnly? StartTime) : IRequest<Result<TimeSlot>>;

public class StartCommandHandler : IRequestHandler<StartCommand, Result<TimeSlot>>
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ITimeSheetRepository repo;

    public StartCommandHandler(IDateTimeProvider dateTimeProvider, ITimeSheetRepository repo)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.repo = repo;
    }

    public Task<Result<TimeSlot>> Handle(StartCommand request, CancellationToken _) => 
        Task.FromResult(repo.Get().GetCurrentDay()
            .Bind(currentDay => currentDay.StartTimeSlot(request.StartTime ?? dateTimeProvider.GetTime()))
            .Tap(() => repo.SaveChanges()));
}