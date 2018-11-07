using MyBlog.Engine.Data;
using MyBlog.Engine.Data.Models;
using MyBlog.Engine.Migrations;
using MyBlog.Strings;
using MyLib.Web.Helpers;
using MyLib.Web.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Engine
{
    public sealed class DataService:IDisposable
    {

        #region Declarations

        private const String CategoriesPropertyName = "Categories";
        private const String PostUrlFormat = "{0}/Post/Details/{1}/{2}/";

        private readonly DataContext _context;

        #endregion

        #region Constructors

        public DataService()
        {
            _context = new DataContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion

        #region Properties

        #endregion

        #region Methodes statiques

        public static void Initilize()
        {
            // Intialization strategy
            // Database.SetInitializer(new CreateDatabaseIfNotExists<DataContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());

            // Forces initialization of database
            using (var context = new DataContext())
            {
                context.Database.Initialize(true);
            }
        }

        #endregion

        #region Gestion des droits à utiliser XML RPC

        /// <summary>
        /// Test if publisher is allowed, create one if it is the first call (configuration, setup)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Boolean PublisherAccessAllowed(String login, String password)
        {
            // Test arguments
            if (String.IsNullOrWhiteSpace(login) 
                || String.IsNullOrWhiteSpace(password)) return false;

            // Test credentials
            Boolean result = TestPublisherCredentials(login, password);

            // All is ok
            if (result) return true;

            // Test if an user exists
            result = _context.Publishers.Any();

            // An user exists, creadentials are wrong
            if (result) return false;

            // This credentials are the very first to be used
            // Create an user with this credentials
            return CreatePublisher(login, password);
        }

        /// <summary>
        /// Test publisher credentials
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Boolean TestPublisherCredentials(String login, String password)
        {
            // Get user to compare
            Publisher publisher = _context.Publishers.FirstOrDefault(c => c.Login.ToUpper() == login.ToUpper());

            // Publisher is available?
            if (publisher == null) return false;

            // Hash password to compare
            String hash = HashHelper.Hash(password, publisher.Salt);

            // Compare hashes
            return hash.Equals(publisher.Password);
        }

        /// <summary>
        /// Create a new publisher
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool CreatePublisher(String login, String password)
        {
            // Get a new hash
            HashResult result = HashHelper.Hash(password);

            // Add publisher with hash
            _context.Publishers.Add(new Publisher
            {
                Login = login,
                Password = result.Hash,
                Salt = result.Salt
            });
            return _context.SaveChanges() > 0;
        }

        #endregion

        #region Categories

        /// <summary>
        /// Get categories ordered by name
        /// </summary>
        /// <returns></returns>
        public Category[] GetCategories()
        {
            return _context.Categories
                .OrderBy(c => c.Name)
                .ToArray();
        }

        /// <summary>
        /// Get naem of category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetCategoryName(Int32 id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id)?.Name;
        }


        /// <summary>
        /// Get categories and creat if not exists
        /// </summary>
        /// <param name="categoriesNames"></param>
        /// <returns></returns>
        public Category[] GetCategoriesAndCreatIfNotExists(String[] categoriesNames)
        {
            return _GetCategoriesAndCreatIfNotExists(categoriesNames).ToArray();
        }

        /// <summary>
        /// Inner Get categories and creat if not exists
        /// </summary>
        /// <param name="categoriesNames"></param>
        /// <returns></returns>
        private IEnumerable<Category> _GetCategoriesAndCreatIfNotExists(String[] categoriesNames)
        {
            if (categoriesNames == null || categoriesNames.Length == 0) yield break;
            Category category;
            foreach(String categoryName in categoriesNames)
            {
                // Get category if exists
                category = _context.Categories.FirstOrDefault(c => c.Name == categoryName);
                // If this category donnot exist, create one
                if (category == null)
                {
                    category = new Category() { Name = categoryName };
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
                yield return category;
            }
        }

        /// <summary>
        /// Return list of ccategories and count post of each
        /// </summary>
        /// <returns></returns>
        public Counter[] GetGateoriesCounters()
        {
            return (from c in _context.Categories
                    where c.Posts.Any(p => p.Published && p.DateCreatedGmt <= DateTime.UtcNow)
                    select new Counter { Id = c.Id, Name = c.Name, Count = c.Posts.Count(p => p.Published && p.DateCreatedGmt <= DateTime.UtcNow) }
                    ).OrderByDescending(c=>c.Count).ToArray();
        }

        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean AddCategory(Category category)
        {
            _context.Categories.Add(category);
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Remove/Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteCategory(Int32 id)
        {
            Category category = new Category { Id = id };
            _context.Categories.Attach(category);
            _context.Entry(category).State = EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Edit a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean EditCategory(Category category)
        {
            _context.Categories.Attach(category);
            var entry = _context.Entry(category);
            entry.State = EntityState.Modified;
            return _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Remove a category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Boolean RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            return _context.SaveChanges() > 0;
        }

        #endregion

        #region Posts

        /// <summary>
        /// Get url for a post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public static String GetPostUrl(IPost post)
        {
            return GetPostUrl(post.Id, post.Title);
        }

        /// <summary>
        /// Get url for a post
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static String GetPostUrl(Int32 id, String title)
        {
            return String.Format(PostUrlFormat,
                Settings.Current.Url,
                id.ToString(),
                UriHelper.ToFriendly(title));
        }

        /// <summary>
        /// Get all  post links for SEO sitemap
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PostLinkWithDate> GetAllPostLink()
        {
            return _context.Posts.Where(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow)
                .Select(c => new PostLinkWithDate { Id = c.Id, Title = c.Title, DatePublishedGmt = c.DateCreatedGmt })
                // Enumerate the content to avoid  lazy load when datacontext is closed
                .AsEnumerable();
        }

        /// <summary>
        /// Count posts available
        /// </summary>
        /// <returns></returns>
        public Int32 Countposts()
        {
            return _context.Posts.Count(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow);
        }

        public Int32 CountpostsAndDrafts()
        {
            return _context.Posts.Count();
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Post GetPost(Int32 id)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Get post by id, with full details and comments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PostWithDetails GetPostWithDetails(Int32 id)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Where(c => c.Id == id)
                .Select(c => new PostWithDetails
                {
                    Id = c.Id,
                    Title = c.Title,
                    DateCreatedGmt = c.DateCreatedGmt,
                    Summary = c.BeginningOfContent,
                    Content = c.ContentIsSplitted
                        ? c.BeginningOfContent + c.EndOfContent
                        : c.BeginningOfContent,
                    Categories = c.Categories,
                    CommentsCount = c.Comments.Count,
                    Comments = c.Comments.Select(
                        cc => new CommentToDisplay
                        {
                            Author = cc.Author.Name,
                            Text = cc.Text,
                            DateCreatedGmt = cc.DateCreatedGmt
                        }
                        ).AsEnumerable()
                }).FirstOrDefault();
        }

        /// <summary>
        /// Get link to a post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PostLink GetPostLink(Int32 id)
        {
            return _context.Posts
                .Where(c => c.Id == id)
                .Select(c => new PostLink { Id = c.Id, Title = c.Title })
                .FirstOrDefault();
        }


        /// <summary>
        /// Get latest posts 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public PostWithSummary[] GetPosts(Int32 offset)
        {
            return GetPosts(offset, Settings.Current.PostQuantityPerPage);
        }

        /// <summary>
        /// Get latest posts 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public PostWithSummary[] GetPosts(Int32 offset,Int32 count)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Where(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow)
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(count)
                .Select(c => new PostWithSummary
                {
                    Id = c.Id,
                    Categories = c.Categories,
                    ContentIsSplitted = c.ContentIsSplitted,
                    DateCreatedGmt = c.DateCreatedGmt,
                    Summary = c.BeginningOfContent,
                    Title = c.Title,
                    CommentsCount = c.Comments.Count
                })
                .ToArray();
        }

        public Post[] GetPostsAndDrafts(Int32 offset, Int32 count)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(count)
                .ToArray();
        }

        /// <summary>
        /// Add a new post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public Boolean AddPost(Post post)
        {
            _context.Posts.Add(post);
            return _context.SaveChanges() > 0;
        }

        public Boolean DeletePost(Int32 id)
        {
            Post post = new Post { Id = id };
            _context.Posts.Attach(post);
            _context.Entry(post).State = EntityState.Deleted;
            return _context.SaveChanges() > 0;
        }

        public Boolean EditPost(Post post)
        {
            // Get categories to have
            Category[] categories = post.Categories.ToArray();
            post.Categories.Clear();

            // Sage the post
            _context.Posts.Attach(post);
            _context.Entry(post).State = EntityState.Modified;
            Boolean saved = _context.SaveChanges() > 0;

            // Get this post for collection updates
            post = _context.Posts
                .Include(CategoriesPropertyName)
                .FirstOrDefault(c => c.Id == post.Id);

            // Get categories sored
            Category[] stored = post.Categories
                .ToArray();

            foreach(var category in stored)
            {
                if (!categories.Any(c => c.Id == category.Id))
                {
                    post.Categories.Remove(category);
                    saved = false;
                }
            }
            foreach (var category in categories)
            {
                if (!stored.Any(c => c.Id == category.Id))
                {
                    post.Categories.Add(category);
                    saved = false;
                }
            }

            return saved || _context.SaveChanges() > 0;
        }

        /// <summary>
        /// Get link to previous post
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public PostLink GetPreviousPost(Int32 postId, DateTime date)
        {
            return (from c in _context.Posts
                    where c.Published && c.DateCreatedGmt < DateTime.UtcNow && c.Id != postId && c.DateCreatedGmt < date
                    orderby c.DateCreatedGmt descending
                    select new PostLink { Id = c.Id, Title = c.Title }
                    ).FirstOrDefault();
        }

        /// <summary>
        ///  Get link to next post
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public PostLink GetNextPost(Int32 postId, DateTime date)
        {
            return (from c in _context.Posts
                    where c.Published && c.DateCreatedGmt < DateTime.UtcNow && c.Id != postId && c.DateCreatedGmt > date 
                    orderby c.DateCreatedGmt ascending
                    select new PostLink { Id = c.Id, Title = c.Title }
                    ).FirstOrDefault();
        }

        #endregion

        #region Archives

        /// <summary>
        /// Get archives links and counters
        /// </summary>
        /// <returns></returns>
        public ArchiveLink[] GetArchives()
        {
            var years = (from p in _context.Posts
                         where p.Published && p.DateCreatedGmt <= DateTime.UtcNow
                         group p by new { p.DateCreatedGmt.Year, p.DateCreatedGmt.Month } into g
                         select new
                         {
                             g.Key.Year,
                             g.Key.Month,
                             Count = g.Count()
                         })
                       .GroupBy(c => c.Year)
                       .OrderByDescending(c=>c.Key)
                    .ToArray();

            if (years.Count() > 2)
            {
                return years.Take(2).SelectMany(y =>
                    y.OrderByDescending(m => m.Month).Select(m =>
                        new ArchiveLink
                        {
                            Id = new ArchiveId(m.Year, m.Month),
                            Count = m.Count
                        }))
                        .Union(
                        years.Skip(2).Select(y =>
                        new ArchiveLink
                        {
                            Id = new ArchiveId(y.Key),
                            Count = y.Sum(m => m.Count)
                        })
                        )
                        .ToArray();
            }
            else
            {
                return years
                    .SelectMany(y => y.OrderByDescending(m => m.Month)
                    .Select(m => new ArchiveLink
                    {
                        Id = new ArchiveId(m.Year, m.Month),
                        Count = m.Count
                    })).ToArray();
            }
        }

        //public Post Get
        /// <summary>
        /// Get posts in categroy
        /// </summary>
        /// <param name="archiveId"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public PostWithoutContent[] GetPostsInArchive(ArchiveId id, Int32 offset)
        {
            Func<Post, Boolean> predicate;

            if (id.Month.HasValue)
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && c.DateCreatedGmt.Year == id.Year && c.DateCreatedGmt.Month == id.Month;
            }
            else
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && c.DateCreatedGmt.Year == id.Year;
            }

            return _context.Posts
                .Include(CategoriesPropertyName)
                .Where(predicate)
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(Settings.Current.PostQuantityPerSearch)
                .Select(c =>
                    new PostWithoutContent
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Categories = c.Categories,
                        DateCreatedGmt = c.DateCreatedGmt
                    }
                ).ToArray();
        }

        /// <summary>
        /// Count posts in category
        /// </summary>
        /// <param name="archiveId"></param>
        /// <returns></returns>
        public Int32 CounPostsInArchive(ArchiveId id)
        {
            Func<Post, Boolean> predicate;

            if (id.Month.HasValue)
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                     && c.DateCreatedGmt.Year == id.Year && c.DateCreatedGmt.Month == id.Month;
            }
            else
            {
                predicate = c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && c.DateCreatedGmt.Year == id.Year;
            }


            return _context.Posts
                .Count(predicate);
        }

        #endregion

        #region Filter on category

        /// <summary>
        /// Get posts in categroy
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public PostWithoutContent[] GetPostsInCategory(Int32 categoryId, Int32 offset)
        {
            return _context.Categories
                .FirstOrDefault(c => c.Id == categoryId)
                ?.Posts.Where(p => p.Published && p.DateCreatedGmt <= DateTime.UtcNow)
                ?.OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(Settings.Current.PostQuantityPerSearch)
                .Select(c =>
                    new PostWithoutContent
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Categories = c.Categories.ToArray(),
                        DateCreatedGmt = c.DateCreatedGmt
                    }
                ).ToArray();
        }

        /// <summary>
        /// Count posts in category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Int32  CounPostsInCategory(Int32 categoryId)
        {
            return _context.Categories
                .FirstOrDefault(c => c.Id == categoryId)
                ?.Posts
                ?.Count() ?? 0;
        }

        #endregion

        #region Search

        /// <summary>
        /// Get posts in Search
        /// </summary>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public PostWithoutContent[] GetPostsInSearch(String query, Int32 offset)
        {
            return _context.Posts
                .Include(CategoriesPropertyName)
                .Where(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && (
                        c.Title.Contains(query)
                        || c.EndOfContent.Contains(query)
                    )
                )
                .OrderByDescending(c => c.DateCreatedGmt)
                .Skip(offset)
                .Take(Settings.Current.PostQuantityPerSearch)
                .Select(c =>
                    new PostWithoutContent
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Categories = c.Categories,
                        DateCreatedGmt = c.DateCreatedGmt
                    }
                ).ToArray();
        }

        /// <summary>
        ///  Count posts in Search
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Int32 CounPostsInSearch(String query)
        {
            return _context.Posts
                .Count(c => c.Published && c.DateCreatedGmt <= DateTime.UtcNow
                    && (
                        c.Title.Contains(query)
                        || c.EndOfContent.Contains(query)
                    )
                );
        }

        #endregion

        #region Users

        /// <summary>
        /// Get user from claims information
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="nameIdentifier"></param>
        /// <returns></returns>
        internal UserProfile GetUser(String issuer, String nameIdentifier)
        {
            return _context.Users.FirstOrDefault(c =>
                c.Issuer == issuer
                && c.NameIdentifier == nameIdentifier);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        internal UserProfile GetUser(Int32 userId)
        {
            return _context.Users.FirstOrDefault(c =>
                c.Id == userId);
        }

        /// <summary>
        /// Edit an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Boolean> EditUser(UserProfile user)
        {
            // test user
            if (user == null) return false;

            Boolean mailChanged;

            // Test insert or update
            if (user.Id == 0)
            {
                mailChanged = true;
                user.EmailValidate = false;
                user.EmailValidationToken = Guid.NewGuid();

                // Insert
                _context.Users.Add(user);
            }
            else
            {
                // Get current mail to know if it was changed
                String email = _context.Users.Where(c => c.Id == user.Id)
                    .Select(c => c.Email)
                    .FirstOrDefault();
                mailChanged = String.Compare(email, user.Email, StringComparison.InvariantCultureIgnoreCase) != 0;

                // Attach data for update
                _context.Users.Attach(user);
                // Get entry
                var entry = _context.Entry(user);
                entry.State = EntityState.Unchanged;
                entry.Property(c => c.Name).IsModified = true;
                entry.Property(c => c.Email).IsModified = true;
                
                // Test if mail changed
                if (mailChanged)
                {
                    user.EmailValidate = false;
                    user.EmailValidationToken = Guid.NewGuid();

                    entry.Property(c => c.EmailValidate).IsModified = true;
                    entry.Property(c => c.EmailValidationToken).IsModified = true;
                }
            }


            // Save
            if (_context.SaveChanges() <= 0) return false;
            if (String.IsNullOrWhiteSpace(user.Email)) return true;

            // If mail changed, send validation mail
            if (mailChanged)
            {
                return await SendValidationMail(user.Id, user.Name, user.Email, user.EmailValidationToken);
            }
            
            return true;
        }

        #endregion

        #region Comments

        /// <summary>
        /// Add comment
        /// </summary>
        /// <param name="text"></param>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<Boolean> AddComment(String text, Int32 userId, Int32 postId)
        {
            // Get entry for post
            Post post = new Post { Id = postId };
            _context.Posts.Attach(post);

            // Get entry for author
            UserProfile user = new UserProfile { Id = userId };
            _context.Users.Attach(user);

            // Create a new comment
            Comment comment = new Comment
            {
                Text = text.Replace(Environment.NewLine,"<br/>"),
                DateCreatedGmt = DateTime.UtcNow,
                Post = post,
                Author = user
            };

            // Add comment to databas
            _context.Comments.Add(comment);
            if (_context.SaveChanges() > 0)
            {
                await NotifyUserForComment(postId, userId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Send mail onh comments subsribers when a comment has been added
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="authorOfComment"></param>
        /// <returns></returns>
        private async Task NotifyUserForComment(Int32 postId, Int32 authorOfComment)
        {
            String authorMail = Settings.Current.AuthorMail;
            String authorName = Settings.Current.AuthorName;

            // Get post informations
            var post = (from p in _context.Posts
                        where p.Id == postId
                        select new
                        {
                            p.Title,
                            // Take only other users with mail validated
                            Users = (from u in p.CommentsFollowers
                                     where u.Id != authorOfComment && u.EmailValidate && !String.IsNullOrEmpty(u.Email) && u.Email != authorMail
                                     group u by u.Email into g
                                     let c = g.FirstOrDefault()
                                     select new { c.Name, c.Email })
                        }
                        ).FirstOrDefault();

            // data null
            if (post == null) return;

            Boolean result = true;

            // Format the mail subject
            String subject = String.Format(Resources.EMailCommentAddedSubject, post.Title);
            String postUri = GetPostUrl(postId,post.Title);

            if (post.Users?.Any() ?? false)
            {
                // Send mails
                foreach (var user in post.Users)
                {
                    result &= await MailService.Send(
                        user.Email,
                        user.Name,
                        subject,
                        String.Format(Resources.EMailCommentAddedContent,
                            user.Name,
                            postUri,
                            post.Title)
                        );
                }
            }
            // Send mail to the author
            result &= await MailService.Send(
                authorMail,
                authorName,
                subject,
                String.Format(Resources.EMailCommentAddedContent,
                    authorName,
                    postUri,
                    post.Title)
                );

            if (!result) Trace.TraceError("Erros when sennding comment notification on post " + postId);
        }

        /// <summary>
        /// Subscrib or unsubscrib
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <param name="subscribtion"></param>
        /// <returns></returns>
        public Boolean SubscribToCommentNotification(Int32 postId, Int32 userId, Boolean subscribtion)
        {
            Post post = _context.Posts
                .Include("CommentsFollowers")
                .FirstOrDefault(c => c.Id == postId);
            if (post == null) return false;

            // Try to know if user as allways subscrib
            Boolean susbribed = post.CommentsFollowers.Any(u => u.Id == userId);

            if (subscribtion && !susbribed)
            {
                // Susbsctrib
                UserProfile user = new UserProfile { Id = userId };
                _context.Users.Attach(user);

                post.CommentsFollowers.Add(user);

                return _context.SaveChanges() > 0;
            }
            else if (susbribed)
            {
                // Unsubscrib
                UserProfile user = post.CommentsFollowers
                    .FirstOrDefault(c => c.Id == userId);

                if (user != null)
                    post.CommentsFollowers.Remove(user);

                return _context.SaveChanges() > 0;
            }
            return true;
        }

        /// <summary>
        /// Know if current user has subscribed to this post comments notifications
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean HasCurrentUserSubscibed(Int32 postId, Int32 userId)
        {
            return _context.Posts.Any(c => c.Id == postId
                && c.CommentsFollowers.Any(u => u.Id == userId));
        }

        #endregion

        #region Mail validation

        /// <summary>
        /// Validate an email
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="validationKey"></param>
        /// <returns></returns>
        public Boolean ValidateMail(Int32 userId, Guid validationToken)
        {
            Boolean ok = _context.Users.Any(c => c.Id == userId && c.EmailValidationToken == validationToken);
            if (ok)
            {
                // User to update
                UserProfile user = new UserProfile
                {
                    Id = userId,
                    EmailValidate = true
                };
                // Attach and mark property that was modified
                _context.Users.Attach(user);
                var entry = _context.Entry(user);
                entry.State = EntityState.Unchanged;
                entry.Property(c => c.EmailValidate).IsModified = true;

                // do not chech others model data
                _context.Configuration.ValidateOnSaveEnabled = false;
                Boolean saved = _context.SaveChanges() > 0;
                _context.Configuration.ValidateOnSaveEnabled = true;

                // Save
                return saved;
            }
            return false;
        }

        /// <summary>
        /// Send an email for validation and save token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Boolean> SendValidationMail(Int32 userId)
        {
            // Get data from this user
            var userData = (from u in _context.Users
                           where u.Id == userId
                           select new { u.Name, u.Email }).FirstOrDefault();
            if (userData == null || String.IsNullOrEmpty(userData.Email)) return false;

            Guid token = Guid.NewGuid();

            // User to update
            UserProfile user = new UserProfile
            {
                Id = userId,
                EmailValidate = false,
                EmailValidationToken = token
            };
            // Attach and mark property that was modified
            _context.Users.Attach(user);
            var entry = _context.Entry(user);
            entry.State = EntityState.Unchanged;
            entry.Property(c => c.EmailValidate).IsModified = true;
            entry.Property(c => c.EmailValidationToken).IsModified = true;

            // do not chech others model data
            _context.Configuration.ValidateOnSaveEnabled = false;
            Boolean saved = _context.SaveChanges() > 0;
            _context.Configuration.ValidateOnSaveEnabled = true;
            // Save 
            if (saved)
            {
                return await SendValidationMail(userId, userData.Name, userData.Email, token);
            }
            return false;
        }

        /// <summary>
        /// Send an email for validation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="mail"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task<Boolean> SendValidationMail(Int32 userId, String name, String mail, Guid token)
        {
            // Save 
            String url = String.Format(
                "{0}/Account/ValidateMail/{1}?token={2}",
                Settings.Current.Url,
                userId.ToString(),
                token.ToString());

            String content = String.Format(Resources.EmailValidationContentFormat,
                    name,
                    Settings.Current.Url,
                    url);

            // Send mail
            return await MailService.Send(
                mail,
                name,
                Resources.EmailValidationSubject,
                content
                );
        }

        #endregion
    }
}
