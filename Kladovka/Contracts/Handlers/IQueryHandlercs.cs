using MediatR;

namespace Kladovka.Contracts.Handlers
{
    public interface IQueryHandler<TRequest> : IRequestHandler<TRequest>
            where TRequest : IRequest
    {
    }
    public interface IQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
    }
}
