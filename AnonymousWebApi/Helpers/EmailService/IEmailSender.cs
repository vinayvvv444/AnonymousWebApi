namespace AnonymousWebApi.Helpers.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        void SendMail();
    }
}