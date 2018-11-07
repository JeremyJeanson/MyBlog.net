using CookComputing.XmlRpc;
using MyBlog.Engine;
using MyBlog.Models.MetaWeblog;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyBlog.Controllers.MetaWeblog
{
    /// <summary>
    /// Service XML-RPL pour MEtaWeblog
    /// </summary>
    [XmlRpcService(Name = "MetaWeblog2")]
    internal sealed class MetaWeblogHandler : XmlRpcService
    {
        private const string Published = "publish";
        private const string Draft = "draft";

        /// <summary>
        /// Translate a database Post in Xml RPC Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private static Post TranslatePost(Engine.Data.Models.Post post)
        {
            String url = DataService.GetPostUrl(post);

            // Return the post
            return new Post
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.GetFullContentForOpenLiveWriter(),
                DateCreatedGmt = post.DateCreatedGmt,
                Status = post.Published ? Published : Draft,
                UserId = MetaWeblogConfiguration.UserId,
                Categories = post.Categories.Select(c => c.Name).ToArray(),
                Url = url,
                PermaLink = url
            };
        }

        /// <summary>
        /// Translate an XML RPC Post in Database Post
        /// </summary>
        /// <param name="db"></param>
        /// <param name="post"></param>
        /// <returns></returns>
        private static Engine.Data.Models.Post TranslatePost(DataService db, PostNew post, Boolean publish)
        {
            // Create a new post
            var result = new Engine.Data.Models.Post
            {
                Title = post.Title,
                DateCreatedGmt = post.DateCreatedGmt ?? post.DateCreated ?? DateTime.UtcNow,
                Published = publish
            };
            result.SetContentFromHtml(post.Description);

            // Get categories
            result.Categories = new Collection<Engine.Data.Models.Category>();
            var categories = db.GetCategoriesAndCreatIfNotExists(post.Categories);
            if (categories?.Length > 0)
            {
                // Add categories
                foreach (var category in categories)
                {
                    result.Categories.Add(category);
                }
            }           

            return result;
        }

        [XmlRpcMethod("metaWeblog.getPost")]
        public Post GetPost(String postid, String userName, String password)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return new Post();
                // Try to get the post by id
                var post = db.GetPost(Int32.Parse(postid));
                if (post == null) return new Post();
                return TranslatePost(post);
            }
        }

        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        public Post[] GetRecentPosts(String blogId, String userName, String password, Int32 numberOfPosts)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return null;
                // Try to get posts
                var posts = db.GetPostsAndDrafts(
                    0,
                    numberOfPosts == 0
                        ? 10
                        : numberOfPosts);
                if (posts == null || posts.Length == 0) return null;
                // Return posts
                return posts.Select(c => TranslatePost(c))
                    .ToArray();
            }
        }

        [XmlRpcMethod("metaWeblog.newPost")]
        public Int32 NewPost(String blogId, String userName, String password, PostNew post,Boolean publish)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return -1;
                // Get a post to insert
                var result = TranslatePost(db, post,publish);
                // Try to insert
                if (!db.AddPost(result)) return -1;
                return result.Id;
            }
        }

        [XmlRpcMethod("metaWeblog.editPost")]
        public Boolean EditPost(String postId, String userName, String password, PostNew post, Boolean publish)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return false;
                // Get a post to insert
                var result = TranslatePost(db, post, publish);
                Int32 id = Int32.Parse(postId);
                if (id > 0)
                {
                    result.Id = id;
                    // Try to edit
                    return db.EditPost(result);
                }
                else
                {
                    return db.AddPost(result);
                }
            }
        }

        [XmlRpcMethod("blogger.deletePost")]
        public Boolean DeletePost(String appKey, String postId, String userName, String password, Boolean publish)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return false;
                return db.DeletePost(Int32.Parse( postId));
            }
        }

        [XmlRpcMethod("metaWeblog.getCategories")]
        public CategoryInfo[] GetCategories(String blogId, String userName, String password)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return null;
                return db.GetCategories()
                    .Select(c => new CategoryInfo
                    {
                        Id = c.Id.ToString(),
                        Name = c.Name,
                        Description= c.Name,
                        ParentId=String.Empty,
                        Rss=String.Empty,
                        Url = c.Url
                    }).ToArray();
            }
        }

        [XmlRpcMethod("metaWeblog.newMediaObject")]
        public MediaObjectInfo NewMediaObject(String blogId, String userName, String password, MediaObject mediaObject)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return new MediaObjectInfo();
                // Try to upload this file
                Uri uri = FilesService.Upload(mediaObject.Name, mediaObject.Bits);
                // Error
                if (uri == null) return new MediaObjectInfo();
                // Else return informations
                return new MediaObjectInfo()
                {
                    Id = mediaObject.Name,
                    Name = mediaObject.Name,
                    Type = mediaObject.Type,
                    Url = uri.ToString()
                };
            }
        }

        [XmlRpcMethod("blogger.getUsersBlogs")]
        public BlogInfo[] GetUsersBlogs(String appkey, String userName, String password)
        {
            using (var db = new DataService())
            {
                if (!db.PublisherAccessAllowed(userName, password)) return null;

                return new BlogInfo[]
                {
                    new BlogInfo
                    {
                        Id = MetaWeblogConfiguration.BlogId,
                        BlogName = Settings.Current.Title,
                        IsAdmin = true,
                        Url = Settings.Current.Url,
                        XmlRpc = MetaWeblogConfiguration.XmlRpcControllerName
                    }
                };
            }
        }
    }
}