namespace EduHomeApp.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string body, List<string> emails, string title, string subject);
    }
}
