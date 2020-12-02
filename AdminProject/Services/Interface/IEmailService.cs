using AdminProject.Models;

namespace AdminProject.Services.Interface
{
    public interface IEmailService
    {
        void SendContactMail(string subject, ContactModelDto contact);
        void SendActivationMail(string userMail, string name, string surname, string code);
        void SendNewPasswordMail(string userMail, string name, string surname, string password);
        void SendForgotPasswordMail(string userMail, string name, string surname, string password);
    }
}