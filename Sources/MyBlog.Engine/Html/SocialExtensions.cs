using MyBlog.Strings;
using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyLib.Web.Html
{
    /// <summary>
    /// Use less style from https://github.com/lipis/bootstrap-social
    /// </summary>
    public static class SocialExtensions
    {
        public enum SocialnetWork
        {
            Twitter = 0,
            Facebook = 1,
            LinkedIn = 2,
            GooglePlus = 3,
            Reddit = 4,
            Pinterest = 5,
            Yahoo = 6,
            Vk = 7,
            Viadeo = 8,
            Yammer = 9
        }

        public static MvcHtmlString Socials(this HtmlHelper htmlHelper, Int32 id, String title, String uri)
        {
            String shareId = id.ToString();

            // Create a new stringbuilder
            StringBuilder sb = new StringBuilder();

            // Add buttons
            AddSocial(sb, "FaceBook", "facebook", "fab fa-facebook-f", shareId);
            AddSocial(sb, "Twitter", "twitter", "fab fa-twitter", shareId);
            AddSocial(sb, "Linked In", "linkedin", "fab fa-linkedin-in", shareId);
            AddSocial(sb, "Google +", "googleplus", "fab fa-google-plus-g", shareId);
            sb.Append("<div class=\"dropdown\">");
            sb.Append("<button class=\"btn dropdown-toggle\" type=\"button\" id=\"menu" + shareId + "\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\" aria-label=\"" + Resources.ShowMoreShareOptions + "\"><i class=\"fa fa-share-alt\" aria-hidden=\"true\"></i></button>");
            sb.Append("<div class=\"dropdown-menu\" aria-labelledby=\"menu" + id + "\">");
            AddSocialDrop(sb, "Yammer", "yammer", "fas fa-share", shareId);
            AddSocialDrop(sb, "Viadéo", "viadeo", "fab fa-viadeo", shareId);
            AddSocialDrop(sb, "Reddit", "reddit", "fab fa-reddit-alien", shareId);
            AddSocialDrop(sb, "Pinterest", "pinterest", "fab fa-pinterest-p", shareId);
            AddSocialDrop(sb, "Yahoo", "yahoo", "fab fa-yahoo", shareId);
            //AddSocial(sb, "VK", "vk", shareId);
            AddMail(sb, title, uri);
            sb.Append("</div>");
            sb.Append("</div>");

            // Return the string
            return new MvcHtmlString(sb.ToString());
        }

        static void AddSocial(StringBuilder sb, String networkName, String networkKey,String fontAwesome, String id)
        {
            sb.Append("<a target=\"_blank\" class=\"btn btn-social-icon btn-" + networkKey + "\" href=\"/Share/?id=" + id + "&N=" + networkKey + "\"><i aria-hidden=\"true\" class=\"" + fontAwesome + "\"></i><span class=\"sr-only\">" + Resources.ShareWith + " " + networkName + "</span></a>");
        }

        static void AddSocialDrop(StringBuilder sb, String networkName, String networkKey, String fontAwesome, String id)
        {
            sb.Append("<a target=\"_blank\" class=\"dropdown-item\" href=\"/Share/?id=" + id + "&N=" + networkKey + "\"><i aria-hidden=\"true\" class=\"" + fontAwesome + "\"></i> <span aria-hidden=\"true\">" + networkName + "</span><span class=\"sr-only\">" + Resources.ShareWith + " " + networkName + "</span></a>");
        }

        static void AddMail(StringBuilder sb, String title,String uri)
        {
            sb.Append("<a target=\"_blank\" class=\"dropdown-item\" href=\"mailto:?subject=" + Uri.EscapeDataString(WebUtility.HtmlDecode(title)) + "&body=" + Uri.EscapeDataString(uri) + "\"><i aria-hidden class=\"fa fa-envelope\" aria-label=\"Mail\"></i> <span aria-hidden>Mail</span><span class=\"sr-only\">" + Resources.ShareWith + " mail</span></a>");
        }
    }
}
