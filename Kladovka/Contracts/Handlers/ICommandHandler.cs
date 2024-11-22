using MediatR;

namespace Kladovka.Contracts.Handlers
{
    public interface ICommandHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
    }

    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
    }
}
