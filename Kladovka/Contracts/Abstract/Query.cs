using MediatR;

namespace Kladovka.Contracts.Abstract
{
    public class Query<TResponse> : IMessage, IRequest<TResponse>
    {
        public Guid CorrelationId { get; set; }
        protected Query() : this(Guid.NewGuid())
        {
        }
        protected Query(Guid correlationId) => CorrelationId = correlationId;
    }

}
