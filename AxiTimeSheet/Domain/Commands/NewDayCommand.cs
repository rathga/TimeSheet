using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.Domain.Commands;

public record NewDayCommand(DateOnly? Date) : IRequest<Result<CurrentDay>>;

public class NewDayCommandHandler : IRequestHandler<NewDayCommand, Result<CurrentDay>>
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ITimeSheetRepository repo;

    public NewDayCommandHandler(IDateTimeProvider dateTimeProvider, ITimeSheetRepository repo)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.repo = repo;
    }

    public Task<Result<CurrentDay>> Handle(NewDayCommand request, CancellationToken cancellationToken) =>
        Task.FromResult(
            repo
                .Get()
                .StartNewDay(request.Date ?? dateTimeProvider.GetDate())
                .Tap(() => repo.SaveChanges()));
}
