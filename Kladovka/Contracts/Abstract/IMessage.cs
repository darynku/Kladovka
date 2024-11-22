namespace Kladovka.Contracts.Abstract;

public interface IMessage
{
    Guid CorrelationId { get; set; }
}
