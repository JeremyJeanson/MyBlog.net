using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MyBlog.Engine
{
    public sealed class MailService
    {
        #region Declarations

        private const String ApiKeyConfigurationKey = "SendGrid";
        private static readonly String ApiKey;

        #endregion

        #region Constructors

        static MailService()
        {
            ApiKey = ConfigurationManager.AppSettings[ApiKeyConfigurationKey];
        }

        #endregion

        #region Properties

        #endregion

        #region Methodes

        /// <summary>
        /// Send plain text mail
        /// </summary>
        /// <param name="receiverMail"></param>
        /// <param name="fromMailMail"></param>
        /// <param name="fromName"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<Boolean> Send(String toMail, String toName, String subject, String content)
        {
            try
            {
                // Get a new client 
                var client = new SendGridClient(ApiKey);
                // Initialize the mail
                var message = new SendGridMessage()
                {
                    From = new EmailAddress(Settings.Current.SendMailFrom, Settings.Current.Title),
                    Subject = subject,
                    HtmlContent = content + "<p> Blog : " + Settings.Current.Url + "</p>"
                };
                message.AddTo(new EmailAddress(toMail, toName));
                var response = await client.SendEmailAsync(message);
                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
        }

        #endregion
    }
}
