using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI;

public interface ICommand : IRequest<Result<string>> { }
public interface IDomainCommand<TRequest, TResponse> : ICommand {}