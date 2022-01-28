namespace Vue.Splash_API.Services.Mail;

public record MailSettings
{
    public string MailUser { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string MailPassword { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
}