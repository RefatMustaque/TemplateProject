using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TemplateProject.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient()
            {
                Host = "mail.alphasoft.com.bd",
                Port = 587,
                EnableSsl = false,
                UseDefaultCredentials=false,
                Credentials = new NetworkCredential("support@alphasoft.com.bd", "123456789")
            };

            // add from,to mailaddresses
            MailAddress from = new MailAddress("support@alphasoft.com.bd", "Alphasoft Tech.");
            MailAddress to = new MailAddress(email, email);

            // add ReplyTo
            MailAddress replyTo = new MailAddress("support@alphasoft.com.bd");

            MailMessage mail = new MailMessage(from, to);
            mail.ReplyToList.Add(replyTo);

            // set subject and encoding
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            mail.Body = htmlMessage;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            mail.IsBodyHtml = true;

            client.Send(mail);

            return Task.CompletedTask;


            //if (!string.IsNullOrEmpty(email))
            //{
            //    var message = new MimeMessage();
            //    message.From.Add(new MailboxAddress("alphasoft19@gmail.com"));
            //    message.To.Add(new MailboxAddress(email));
            //    message.Subject = subject;
            //    message.Body = new TextPart("html")
            //    {
            //        Text = htmlMessage
            //    };
            //    using (var client = new SmtpClient())
            //    {
            //        client.Connect("smtp.gmail.com", 587, false);
            //        client.Authenticate("alphasoft19@gmail.com", "Ab@12345");
            //        client.Send(message);
            //        client.Disconnect(true);
            //    }
            //    return Task.CompletedTask;
            //}
            //throw new NotImplementedException();
        }
    }
}
