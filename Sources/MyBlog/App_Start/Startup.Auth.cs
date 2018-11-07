using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using MyBlog.Engine;
using Owin;
using System;
using System.Configuration;
using System.Web;

namespace MyBlog
{
    partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Provider to avoid api redirection
            var provider = new CookieAuthenticationProvider();
            var originalHandler = provider.OnApplyRedirect;
            provider.OnApplyRedirect = context =>
            {
                if (!context.Request.Uri.LocalPath.StartsWith(VirtualPathUtility.ToAbsolute("~/api")))
                {
                    context.RedirectUri = new Uri(context.RedirectUri).PathAndQuery;
                    originalHandler.Invoke(context);
                }
            };

            // Options
            var options = new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/Account/Login"),
                Provider = provider
            };

            // Add options
            app.UseCookieAuthentication(options);

            // Set default authentitationtype
            app.SetDefaultSignInAsAuthenticationType(options.AuthenticationType);
            
            //// Basic
            //app.UseBasicAuthentication(new BasicAuthenticationOptions(
            //    "AtomPub",
            //    BasicAuthentication));

            // Add Microsoft Account Authentication
            if (Settings.Current.MicrosoftAccountAuthentication)
            {
                String id = ConfigurationManager.AppSettings["MicrosoftAccountAuthenticationClientId"];
                String secret = ConfigurationManager.AppSettings["MicrosoftAccountAuthenticationSecret"];

                if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(secret))
                {
                    app.UseMicrosoftAccountAuthentication(id, secret);
                }
            }

            // Add Twitter Authentication
            if (Settings.Current.TwitterAuthentication)
            {
                String id = ConfigurationManager.AppSettings["TwitterAuthenticationClientId"];
                String secret = ConfigurationManager.AppSettings["TwitterAuthenticationSecret"];

                if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(secret))
                {
                    app.UseTwitterAuthentication(id, secret);
                }
            }

            // Add FaceBook Authentication
            if (Settings.Current.FacebookAuthentication)
            {
                String id = ConfigurationManager.AppSettings["FacebookAuthenticationClientId"];
                String secret = ConfigurationManager.AppSettings["FacebookAuthenticationSecret"];

                if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(secret))
                {
                    app.UseFacebookAuthentication(id, secret);
                }
            }

            // Add Google Plus Authentication
            if (Settings.Current.GoogleAuthentication)
            {
                String id = ConfigurationManager.AppSettings["GoogleAuthenticationClientId"];
                String secret = ConfigurationManager.AppSettings["GoogleAuthenticationSecret"];

                if (!String.IsNullOrEmpty(id) && !String.IsNullOrEmpty(secret))
                {
                    app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                    {
                        ClientId = id,
                        ClientSecret = secret
                    });
                }
            }
        }

        ///// <summary>
        ///// Authentication for AtomPub
        ///// </summary>
        ///// <param name="login"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //private static async Task<Claim[]> BasicAuthentication(String login, String password)
        //{

        //    using (var db = new DataService())
        //    {
        //        if (await db.PublisherAccessAllowedAsync(login, password))
        //        {
        //            return new []
        //            {
        //                new Claim(ClaimTypes.NameIdentifier, login),
        //                new Claim(ClaimTypes.Role, "Publisher")
        //            };
        //        }
        //    }
        //    return null;
        //}
    }
}