using System.Net;
using System.Net.Mail;
using System.Web;

namespace stayWithMeApi.Services
{
    public class EmailService(IConfiguration configuration)
    {
        MailAddress fromAddress = new MailAddress(configuration["EmailSetting:FromEmail"], "Archiv AI");
        SmtpClient smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(configuration["EmailSetting:FromEmail"], configuration["EmailSetting:FromPassword"])
        };
        public async void SendOtpMail(string otp, string email)
        {

            MailAddress toAddress = new MailAddress(email);
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "ArchivAi change password otp",
                IsBodyHtml = false, //F_T will make design later 
                Body = otp
            })
            {
                smtp.Send(message);
            }
        }
    }
}
