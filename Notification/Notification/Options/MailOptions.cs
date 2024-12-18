namespace Notification.Options
{
    public class MailOptions
    {
        public const string ConfigureSectionName = "Mail";
        public string From { get; init; } = string.Empty;
        public string Subject { get; init; } = string.Empty;
        public string Host{ get; init; } = string.Empty;
        public string Port { get; init; } = string.Empty;
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public bool EnablSsl { get; init; }
    }
}
