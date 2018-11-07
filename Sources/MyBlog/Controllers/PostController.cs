using MyBlog.Engine;
using MyBlog.Engine.Data.Models;
using MyBlog.Strings;
using MyBlog.Models;
using MyLib.Web;
using MyLib.Web.Helpers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class PostController : Controller
    {
        #region Declarations

        private const String IndexGetMoreView = "_IndexGetMore";
        private const String FilterView = "Filter";
        private const String FilterGetMoreView = "_FilterGetMore";
        private const String SearchGetMoreView = "_SearchGetMore";

        #endregion

        #region Posts

        /// <summary>
        /// Default Action (get posts) 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ActionResult Index(Posts model)
        {
            // Initialize the model
            InitializePosts(model);
            return View(model);
        }

        public PartialViewResult IndexGetMore(Posts model)
        {
            // Initialize the model
            InitializePosts(model);
            return PartialView(IndexGetMoreView, model);
        }

        /// <summary>
        /// Initialize the model for posts
        /// </summary>
        /// <param name="model"></param>
        private static void InitializePosts(Posts model)
        {
            using (var db = new DataService())
            {
                // Get items
                model.Items = db.GetPosts(model.Page * Settings.Current.PostQuantityPerPage);

                // Get Counter to know if we have more items to load
                Int32 count = db.Countposts();
                model.Available = count;
                model.HaveMoreResults = count > (model.Page + 1) * Settings.Current.PostQuantityPerPage;

                // Update offset
                model.NextPage = model.Page + 1;
            }
        }

        #endregion

        #region Categories

        /// <summary>
        /// Filter post by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Category(PostsFilter model)
        {
            // Filters have to share the id
            if (Int32.TryParse(model.Id, out int id))
            {
                using (var db = new DataService())
                {
                    // Title
                    model.Title = Resources.Category;

                    // Get name
                    model.SubTitle = db.GetCategoryName(id);
                    model.Description = String.Format(
                        Resources.CategoryDescription,
                        model.SubTitle);

                    // Initialize the data
                    InitializeCategorymodel(db, model, id);
                }
            }
            return View(FilterView, model);
        }

        /// <summary>
        /// Get more items when filter posts by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult CategoryGetMore(PostsFilter model)
        {
            // Filters have to share the id
            if (Int32.TryParse(model.Id, out int id))
            {
                using (var db = new DataService())
                {
                    InitializeCategorymodel(db, model, id);
                }
            }
            return PartialView(FilterGetMoreView, model);
        }

        /// <summary>
        /// Add data on model used by category
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        private static void InitializeCategorymodel(DataService db, PostsFilter model,Int32 id)
        {
            // Action
            model.Action = "Category";

            // Gets posts
            model.Items = db.GetPostsInCategory(id, model.Page * Settings.Current.PostQuantityPerSearch);

            // Have more
            Int32 count = db.CounPostsInCategory(id);
            model.Available = count;
            model.HaveMoreResults = count > (model.Page + 1) * Settings.Current.PostQuantityPerSearch;

            // Update page index
            model.NextPage = model.Page + 1;
        }

        #endregion

        #region Details

        /// <summary>
        /// Get detail of post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Int32 id)
        {
            using (var db = new DataService())
            {
                // Initialize mode lwith post from database
                Details model = new Details()
                {
                    Post = db.GetPostWithDetails(id)
                };
                // Post not found
                if (model.Post == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    // Get description
                    model.Description = WebpageHelper.GetMetaDescrition(model.Post.Summary);

                    // Get previous and next posts informations, to display prevous and next buttons
                    DateTime date = model.Post.DateCreatedGmt;

                    model.PreviousPost = db.GetPreviousPost(id, date);
                    model.NextPost = db.GetNextPost(id, date);

                    // Build comment model to allow user to comment this post
                    UserProfile user = UserService.Get(db);
                    if (user == null)
                    {
                        model.Comment = null;
                        model.CurrentUserSubscibed = false;
                    }
                    else
                    {
                        model.Comment = new Comment();
                        model.CurrentUserSubscibed = db.HasCurrentUserSubscibed(id, user.Id);
                    }
                }

                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Comment(Details model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DataService())
                {
                    // User id
                    Int32 userId = UserService.Get()?.Id ?? 0;

                    // Create the comment
                    await db.AddComment(model.Comment.Text, userId, model.Post.Id);
                }
            }
            return RedirectToAction("Details", new { id = model.Post.Id });
        }

        [Authorize]
        [HttpPost]
        public PartialViewResult SubscribToCommentNotification(Int32 id, Boolean subscription)
        {
            Boolean model;
            using(var db = new DataService())
            {
                // User id
                Int32 userId = UserService.Get()?.Id ?? 0;

                // Subscrib or unsubscribe
                model = db.SubscribToCommentNotification(id, userId, subscription);
            }
            return PartialView("_SubscribToCommentNotification", model);
        }

        #endregion

        #region Archives

        /// <summary>
        /// Filter post by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Archive(PostsFilter model)
        {
            ArchiveId id = new ArchiveId(model.Id);
            using (var db = new DataService())
            {
                // Title
                model.Title = Resources.Archives;

                // Get name
                model.SubTitle = id.ToString();
                model.Description = String.Format(
                    Resources.ArchivesDescription,
                    model.SubTitle);

                // Initialize the data
                InitializeArchiveModel(db, model,id);
            }
            return View(FilterView, model);
        }

        /// <summary>
        /// Get more items when filter posts by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult ArchiveGetMore(PostsFilter model)
        {
            ArchiveId id = new ArchiveId(model.Id);
            using (var db = new DataService())
            {
                InitializeArchiveModel(db, model, id);
            }
            return PartialView(FilterGetMoreView, model);
        }

        private static void InitializeArchiveModel(DataService db, PostsFilter model, ArchiveId id)
        {
            // Archive
            model.Action = "Archive";

            // Gets posts
            model.Items = db.GetPostsInArchive(id, model.Page * Settings.Current.PostQuantityPerSearch);

            // Have more
            Int32 count = db.CounPostsInArchive(id);
            model.Available = count;
            model.HaveMoreResults = count > (model.Page + 1) * Settings.Current.PostQuantityPerSearch;

            // Update page index
            model.NextPage = model.Page + 1;
        }

        #endregion

        #region Search

        /// <summary>
        /// Filter post by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Search(SearchFilter model)
        {
            using (var db = new DataService())
            {
                // Title
                model.Title = Resources.Search;

                // Get name
                model.SubTitle = model.Query;
                model.Description = String.Format(
                    Resources.SearchDescription,
                    model.SubTitle);

                // Initialize the data
                InitializeSearchModel(db, model);
            }
            return View(model);
        }

        /// <summary>
        /// Get more items when filter posts by category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PartialViewResult SearchGetMore(SearchFilter model)
        {
            using (var db = new DataService())
            {
                InitializeSearchModel(db, model);
            }
            return PartialView(SearchGetMoreView, model);
        }

        private static void InitializeSearchModel(DataService db, SearchFilter model)
        {
            // Archive
            model.Action = "Search";

            if (String.IsNullOrWhiteSpace(model.Query))
            {
                model.Items = null;
                model.Available = 0;
                model.HaveMoreResults = false;
                model.NextPage = 0;
            }
            else
            {
                // Gets posts
                model.Items = db.GetPostsInSearch(model.Query, model.Page * Settings.Current.PostQuantityPerSearch);

                // Have more
                Int32 count = db.CounPostsInSearch(model.Query);
                model.Available = count;
                model.HaveMoreResults = count > (model.Page + 1) * Settings.Current.PostQuantityPerSearch;

                // Update page index
                model.NextPage = model.Page + 1;
            }
        }

        #endregion
    }
}