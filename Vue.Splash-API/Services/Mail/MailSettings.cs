namespace Vue.Splash_API.Services.Mail;

public record MailSettings
{
    public string MailUser { get; init; }
    public string DisplayName { get; init; }
    public string MailPassword { get; init; }
    public string Host { get; init; }
    public int Port { get; init; }
}