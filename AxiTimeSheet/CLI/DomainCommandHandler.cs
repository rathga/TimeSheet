using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

public abstract class DomainCommandHandler<TCommand, TRequest, TResponse> : IRequestHandler<TCommand, Result<string>> 
    where TCommand : IDomainCommand<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    protected DomainCommandHandler(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<Result<string>> Handle(TCommand command, CancellationToken cancellationToken) => 
        MapResponse(command, await mediator.Send(mapper.Map<TRequest>(command), cancellationToken));

    public abstract Result<string> MapResponse(TCommand command, TResponse result);
}