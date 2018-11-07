using MyBlog.Engine;
using MyBlog.Engine.Data.Models;
using MyBlog.Strings;
using MyLib.Web;
using MyLib.Web.Helpers;
using System;

namespace MyBlog.Models
{
    public sealed class LayoutViewModel
    {
        #region Declarations

        private static readonly string _version;

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        static LayoutViewModel()
        {
            _version = ApplicationHelper.GetVersion() + " " + Resources.VersionSuffix;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private LayoutViewModel()
        {
            using (var db = new DataService())
            {
                Categories = db.GetGateoriesCounters();
                Archives = db.GetArchives();
                User = UserService.Get(db);
                UserSettings = UserSettingsService.Get();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///  counters
        /// </summary>
        public Counter[] Categories { get; }

        /// <summary>
        /// Archives
        /// </summary>
        public ArchiveLink[] Archives { get; }

        /// <summary>
        /// Version
        /// </summary>
        public String Version => _version;

        /// <summary>
        /// User loged in
        /// </summary>
        public UserProfile User { get; }

        /// <summary>
        /// User's settings
        /// </summary>
        public UserSettings UserSettings { get; }

        #endregion

        #region Methodes

        /// <summary>
        /// Get an instance for the layout
        /// </summary>
        /// <returns></returns>
        public static LayoutViewModel Get()
        {
            return new LayoutViewModel();
        }

        #endregion
    }
}