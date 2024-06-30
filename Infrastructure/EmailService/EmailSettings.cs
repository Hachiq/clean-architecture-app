namespace Infrastructure.EmailService
{
    public class EmailSettings
    {
        public const string Section = "EmailSettings";

        public string SenderEmail { get; set; } = string.Empty;

        public string SenderName { get; set; } = string.Empty;

        public SmtpSettings SmtpSettings { get; set; } = null!;
    }
}