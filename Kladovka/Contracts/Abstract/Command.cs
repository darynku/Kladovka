using MediatR;

namespace Kladovka.Contracts.Abstract
{
    public abstract class CommandBase : IMessage
    {
        protected CommandBase(Guid correlationId) => CorrelationId = correlationId;
        public Guid CorrelationId { get; set; }
    }
    public abstract class Command : CommandBase, IRequest
    {
        protected Command() : base(Guid.NewGuid())
        {
        }
        protected Command(Guid correlationId) : base(correlationId)
        {
        }
    }
    public abstract class Command<TResponse> : CommandBase, IRequest<TResponse>
    {
        protected Command() : base(Guid.NewGuid())
        {
        }
        protected Command(Guid correlationId) : base(correlationId)
        {
        }
    }

}
