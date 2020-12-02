using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly RuntimeSettings _setting;

        public EmailService(RuntimeSettings setting)
        {
            _setting = setting;
        }
        
        public void SendUserMail(string userMail, string subject, string body)
        {
            EmailSending(userMail, subject, body);
        }

        public void SendContactMail(string subject, ContactModelDto contact)
        {
            var mailTemplate = "<html><body style=\"font-family: calibri; font-size: 15px; color: #000000;\"><table style=\"width:100%;\">" +
                               $"<tr><td>Name Surname</td><td>{contact.NameSurname}</td></tr>" +
                               $"<tr><td>E-Mail</td><td>{contact.Email}</td></tr>" +
                               $"<tr><td>Phone</td><td>{contact.Phone}</td></tr>" +
                               $"<tr><td>Subject</td><td>{contact.Subject}</td></tr>" +
                               $"<tr><td>Message</td><td>{contact.Message}</td></tr>" +
                               "</table></body></html>";

            EmailSending(_setting.ContactAddress, subject, mailTemplate);
        }

        public void SendNewPasswordMail(string userMail, string name, string surname, string password)
        {
            var mailTemplate = $"<html><body style=\"font-family: calibri; font-size: 15px; color: #000000;\">Merhaba {name} {surname} <br>Geçerli şifreniz: <strong>{password}</strong><br><br>İyi günler... </body></html>";

            EmailSending(userMail, "Şifre Yenileme", mailTemplate);
        }

        public void SendForgotPasswordMail(string userMail, string name, string surname, string password)
        {
            var mailTemplate = $"<html><body style=\"font-family: calibri; font-size: 15px; color: #000000;\">Merhaba {name} {surname} <br>Geçerli şifreniz: <strong>{password}</strong><br><br>İyi günler... </body></html>";

            EmailSending(userMail, "Şifre Hatırlatma", mailTemplate);
        }

        public void SendActivationMail(string userMail, string name, string surname, string code)
        {
            var mailTemplate = GetActivationEmailTemplate(name, surname, code);

            EmailSending(userMail, "Hesap Aktivasyonu/ Account Activation", mailTemplate);
        }

        private string GetActivationEmailTemplate(string name, string surname, string code)
        {
            const string body =
                "<html>" +
                "<body style=\"font-family: calibri; font-size: 15px; color: #000000;\">" +
                    "<div>Merhaba {0} {1},<br />Hesabınızı aktif etmek için <a href=\"{2}/kullanici/aktivasyon/{3}\">tıklayınız</a>.</div>" +
                    "<br />" +
                    "<div>Hello {0} {1},<br /> To activate your account please <a href=\"{2}/kullanici/aktivasyon/{3}\">click</a>.</div>" +
                "</body>" +
                "</html>";

            return
                string.Format(body, name, surname, _setting.Domain, code);
        }

        private void EmailSending(string sendMail, string subject, string mailBody)
        {
            var email = new MailMessage
            {
                From = new MailAddress(_setting.EmailAddress),
                Subject = subject,
                //IsBodyHtml = true,
                BodyEncoding = Encoding.GetEncoding("utf-8")
            };
            email.To.Add(sendMail);

            var plainBody = Regex.Replace(mailBody, @"<(.|\n)*?>", string.Empty);
            var plainView = AlternateView.CreateAlternateViewFromString(plainBody, null, "text/plain");
            email.AlternateViews.Add(plainView);

            var htmlView = AlternateView.CreateAlternateViewFromString(mailBody, null, "text/html");
            email.AlternateViews.Add(htmlView);

            var smtp = new SmtpClient
            {
                Credentials = new NetworkCredential(_setting.EmailAddress, _setting.EmailPassword),
                Port = _setting.Port,
                Host = _setting.Smtp,
                EnableSsl = false
            };
            smtp.Send(email);
        }
    }
}