namespace Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string receiver, string subject, string message);
    }
}