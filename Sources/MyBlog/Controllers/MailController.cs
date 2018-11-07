using MyBlog.Engine;
using MyBlog.Models;
using MyBlog.Strings;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class MailController : Controller
    {
        private const String SendedView = "_Sended";

        /// <summary>
        ///  get Fomr
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Create model
            Mail model;

            // Try to get user logged
            var user = UserService.Get();

            // If user is available initialize mail
            model = user == null
                ? new Mail()
                : new Mail
                {
                    SenderMail = user.Email,
                    SenderName = user.Name
                };

            return View(model);
        }

        //Send mail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Mail model)
        {
            if (ModelState.IsValid)
            {
                // Try to send mail
                if (await MailService.Send(
                    Settings.Current.AuthorMail,
                    Settings.Current.AuthorName,
                    model.Subject,
                    String.Format("<p>{0} - {1}</p><p>{2}</p>",
                        model.SenderMail,
                        model.SenderName,
                        model.Content.Replace(Environment.NewLine, "<br/>"))
                        ))
                {
                    // Mail sended
                    return View(SendedView);
                }
                else
                {
                    // eeror when trying to send mail
                    ModelState.AddModelError(String.Empty, Resources.MailSendError);
                    return View(model);
                }
            }
            return View(model);
        }
    }
}