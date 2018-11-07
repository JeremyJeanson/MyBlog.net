using MyBlog.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine
{
    /// <summary>
    /// Blog Feed (for RSS or ATOM)
    /// </summary>
    public sealed class FeedService
    {
        private const String MoreContentButtonFormat = "<p><a href=\"{0}\">{1} {2}</a></p>";

        /// <summary>
        /// Get the blog feed
        /// </summary>
        /// <returns></returns>
        public static SyndicationFeed Get()
        {
            SyndicationFeed feed = new SyndicationFeed(
            Settings.Current.Title,
            Settings.Current.SubTitle,
            new Uri(Settings.Current.Url));

            using (var db = new DataService())
            {
                var posts = db.GetPosts(0, 10);
                if (posts != null && posts.Length > 0)
                {
                    // Create a new list of items
                    var items = (
                        from p in posts
                            // Let uri to use 2 times in the select
                        let uri = DataService.GetPostUrl(p)
                        select new SyndicationItem(
                            p.Title,
                            new TextSyndicationContent(
                                p.ContentIsSplitted
                                ? p.HtmlSummary + String.Format(MoreContentButtonFormat, uri, Resources.ReadMore, Resources.ReadMoreAboutSuffix)
                                : p.HtmlSummary,
                                TextSyndicationContentKind.XHtml),
                            new Uri(uri),
                            p.Id.ToString(),
                            p.DateCreatedGmt)
                    ).ToArray();
                    // Add items
                    feed.Items = items;
                }
            }
            return feed;
        }
    }
}
