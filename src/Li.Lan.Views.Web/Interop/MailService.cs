using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Li.Lan.Views.Web.Interop
{
    public interface IMailService
    {
        void SendMail(string toAddress, string subject, string body);

        void SendPasswordResetRequestEmail(string userName, string passwordVerificationToken);
    }

    public class MailService : IMailService
    {
        public MailService(
            bool sendEmailActive,
            string siteRootUri,
            string smtpHostUri,
            int smtpHostPort,
            string smtpUserName,
            string smtpPassword,
            string fromAddress)
        {
            this.SendEmailActive = sendEmailActive;
            this.SiteRootUri = siteRootUri;
            this.SmtpHostUri = smtpHostUri;
            this.SmtpHostPort = smtpHostPort;
            this.SmtpUserName = smtpUserName;
            this.SmtpPassword = smtpPassword;
            this.FromAddress = fromAddress;
        }

        private bool SendEmailActive { get; set; }

        private string SiteRootUri { get; set; }

        private string SmtpHostUri { get; set; }

        private int SmtpHostPort { get; set; }

        private string SmtpUserName { get; set; }

        private string SmtpPassword { get; set; }

        private string FromAddress { get; set; }

        public void SendMail(string toAddress, string subject, string body)
        {
            if (!this.SendEmailActive)
                return;

            var fromMailAddress = new MailAddress(this.FromAddress, "Liberty Iowa");
            var toMailAddress = new MailAddress(toAddress);
            
            using (var smtp = new SmtpClient(this.SmtpHostUri, this.SmtpHostPort))
            {
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                
                smtp.Credentials = new NetworkCredential(this.SmtpUserName, this.SmtpPassword);

                using (var message = new MailMessage(fromMailAddress, toMailAddress))
                {
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    smtp.Send(message);
                }
            }
        }

        public void SendPasswordResetRequestEmail(string userName, string passwordVerificationToken)
        {
            var passwordResetRequestUri = this.CreatePasswordResetRequestUri(passwordVerificationToken);

            var subject = "Liberty Iowa - Password Reset Request";

            var body = String.Format(
@"<html>
<div style=""background-color: #FFFFFF; color: #333333; font-size: 12pt; font-family: 'Segoe UI Light' , 'Segoe UI' , Arial, Verdana, Helvetica, Sans-Serif"">
A password reset request was submitted for the account associated with this email address. If you would like to reset your password, please follow the link below, if not, please take no action.&nbsp;<br />
&nbsp;<br />
<span style='font-family: ""Courier New""'><a href=""{0}"">{0}</a>&nbsp;<br /></span>
&nbsp;<br />
&nbsp;<br />
&nbsp;<br />
<span style=""font-size: 8pt; color: #999999;"">Copyright &copy; {1} <a href=""http://www.libertyiowa.com/"" style=""color: #999999; text-decoration: none;"" >Liberty Iowa</a>. All rights reserved.</span>
</div>
</html>",
                passwordResetRequestUri,
                DateTime.Now.Year);

            this.SendMail(userName, subject, body);
        }

        private string CreatePasswordResetRequestUri(string passwordVerificationToken)
        {
            return String.Format("https://{0}/Account/PasswordResetRequest?t={1}", this.SiteRootUri, passwordVerificationToken);
        }
    }
}