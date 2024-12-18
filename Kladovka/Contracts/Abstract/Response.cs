namespace Kladovka.Contracts.Abstract
{
    public class Response
    {
        public bool Success { get; init; }
        public string Message { get; init; } = string.Empty;
        public int StatusCode { get; init; }
    }
}
