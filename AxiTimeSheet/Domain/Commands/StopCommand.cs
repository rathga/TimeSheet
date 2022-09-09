using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.Domain.Commands;

public record StopCommand(TimeOnly? StopTime) : IRequest<Result<TimeSlot>>;

public class StopCommandHandler : IRequestHandler<StopCommand, Result<TimeSlot>>
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ITimeSheetRepository repo;

    public StopCommandHandler(
        IDateTimeProvider dateTimeProvider,
        ITimeSheetRepository repo)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.repo = repo;
    }

    public Task<Result<TimeSlot>> Handle(StopCommand request, CancellationToken _) => 
        Task.FromResult(repo.Get().GetCurrentDay()
            .Bind(currentDay => currentDay.StopTimeSlot(request.StopTime ?? dateTimeProvider.GetTime()))
            .Tap(() => repo.SaveChanges()));
}