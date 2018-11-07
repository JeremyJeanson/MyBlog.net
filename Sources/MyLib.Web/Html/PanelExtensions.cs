using System;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace MyLib.Web.Html
{
    public static class PanelExtensions
    {
        /// <summary>
        /// Création d'une Panel bootstrap à utiliser dans un block using
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcPanel BeginPanel(this HtmlHelper htmlHelper)
        {
            return new Html.MvcPanel(htmlHelper, null, null, true);
        }        

        /// <summary>
        /// Création d'une Panel bootstrap à utiliser dans un block using
        /// </summary>
        /// <param name="htmlHelper">Helper</param>
        /// <param name="title">titre affiché sur le panel</param>
        /// <returns></returns>
        public static MvcPanel BeginPanel(this HtmlHelper htmlHelper, String title)
        {
            return new Html.MvcPanel(htmlHelper, title, null, true);
        }

        public static MvcPanel BeginPanel(this HtmlHelper htmlHelper, Boolean withBody)
        {
            return new Html.MvcPanel(htmlHelper, null, null, withBody);
        }

        /// <summary>
        /// Création d'une Panel bootstrap à utiliser dans un block using
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcPanel BeginPanel(this HtmlHelper htmlHelper, String title, String id)
        {
            return new Html.MvcPanel(htmlHelper, title, id, true);
        }

        public static MvcPanel BeginPanel(this HtmlHelper htmlHelper, String title, String id, Boolean withBody)
        {
            return new Html.MvcPanel(htmlHelper, title, id, withBody);
        }

        /// <summary>
        /// Création d'une Panel bootstrap à utiliser dans un block using
        /// </summary>
        /// <param name="htmlHelper">Helper</param>
        /// <param name="title">titre affiché sur le panel</param>
        /// <returns></returns>
        public static MvcPanel BeginPanel(this HtmlHelper htmlHelper, String title, Boolean withBody)
        {
            return new Html.MvcPanel(htmlHelper, title, null, withBody);
        }

        /// <summary>
        /// Begin article
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static MvcArticlePanel BeginArticle(this HtmlHelper htmlHelper, String title)
        {
            return new MvcArticlePanel(htmlHelper, title, String.Empty);
        }

        /// <summary>
        /// Begin article
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static MvcArticlePanel BeginArticle(this HtmlHelper htmlHelper, String title, String uri)
        {
            return new MvcArticlePanel(htmlHelper, title, uri);
        }
    }

    /// <summary>
    /// Panel bootstrap pouvant être utilisé dans un block using d'un vue MVC
    /// </summary>
    public class MvcPanel : IDisposable
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly Boolean _withBody;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title">Titre du panel</param>
        public MvcPanel(HtmlHelper htmlHelper, String title, String id, Boolean withBody)
        {
            // mémorisation du helper html pour pouvoir fermer les balises par la suite lors du dispose()
            _htmlHelper = htmlHelper;
            _withBody = withBody;

            // Création du retour
            StringBuilder result = new StringBuilder();

            // Création du container
            var container = new TagBuilder("div");
            if (!String.IsNullOrEmpty(id))
            {
                container.GenerateId(id);
            }
            container.AddCssClass("card");
            // Bottom margin
            container.AddCssClass("mb-3");

            result.Append(container.ToString(TagRenderMode.StartTag));

            if (!String.IsNullOrEmpty(title))
            {
                // Création du container du header
                var headerContainer = new TagBuilder("div");
                headerContainer.AddCssClass("card-header");

                // Création du header
                var header = new TagBuilder("h2");
                header.InnerHtml = title;

                // Ajout du header à son container
                headerContainer.InnerHtml = header.ToString();
                result.Append(headerContainer.ToString());
            }
            // Création du container du body

            if (withBody)
            {
                var bodyContainer = new TagBuilder("div");
                bodyContainer.AddCssClass("card-body");
                result.Append(bodyContainer.ToString(TagRenderMode.StartTag));
            }            

            _htmlHelper.ViewContext.Writer.Write(result.ToString());
        }

        /// <summary>
        /// Dispose, met fin au block
        /// </summary>
        public void Dispose()
        {
            if (_withBody)
            {
                _htmlHelper.ViewContext.Writer.Write("</div>");
            }
            _htmlHelper.ViewContext.Writer.Write("</div>");
        }
    }

    public class MvcArticlePanel : IDisposable
    {
        private readonly HtmlHelper _htmlHelper;

        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="title">Titre du panel</param>
        public MvcArticlePanel(HtmlHelper htmlHelper, String title, String uri)
        {
            // mémorisation du helper html pour pouvoir fermer les balises par la suite lors du dispose()
            _htmlHelper = htmlHelper;

            // Création du retour
            StringBuilder result = new StringBuilder();

            // Création du container
            var container = new TagBuilder("div");
            container.AddCssClass("card");
            // Bottom margin
            container.AddCssClass("mb-3");

            result.Append(container.ToString(TagRenderMode.StartTag));

            // Création du container du body
            var bodyContainer = new TagBuilder("article");
            bodyContainer.AddCssClass("card-body");
            if (!String.IsNullOrEmpty(title))
            {
                bodyContainer.MergeAttribute("aria-label", WebUtility.HtmlDecode(title));
            }
            result.Append(bodyContainer.ToString(TagRenderMode.StartTag));

            // Creation of header
            var header = new TagBuilder("h2");            
            header.AddCssClass("card-title");

            // Test if uri is evailable
            if (String.IsNullOrEmpty(uri))
            {
                header.InnerHtml = title;
            }
            else
            {
                var anchor = new TagBuilder("a");
                anchor.MergeAttribute("href", uri);
                anchor.InnerHtml = title;
                header.InnerHtml = anchor.ToString();
            }
            // Add header
            result.Append(header.ToString());

            _htmlHelper.ViewContext.Writer.Write(result.ToString());
        }

        /// <summary>
        /// Dispose, met fin au block
        /// </summary>
        public void Dispose()
        {
            _htmlHelper.ViewContext.Writer.Write("</article></div>");
        }
    }
}
