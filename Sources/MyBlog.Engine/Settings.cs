using MyLib.Web.Helpers;
using System;
using System.Configuration;

namespace MyBlog.Engine
{
    public sealed class Settings
    {
        #region Singleton

        private static readonly Settings _current;

        /// <summary>
        /// 
        /// </summary>
        static Settings()
        {
            _current = new Settings();
        }

        /// <summary>
        /// Current instance
        /// </summary>
        public static Settings Current
        {
            get { return _current; }
        }

        #endregion

        #region Declarations


        private const String SendMailFromKey = "SendMailFrom";
        private const String UriKey = "BlogUri";
        private const String TitleKey = "BlogTitle";
        private const String SubTitleKey = "BlogSubTitle";
        private const String AuthorNameKey = "BlogAuthorName";
        private const String AuthorMailKey = "BlogAuthorMail";
        private const String PostQuantityPerPageKey = "PostQuantityPerPage";
        private const String PostQuantityPerSearchKey = "PostQuantityPerSearch";
        private const String MicrosoftAccountAuthenticationkey = "MicrosoftAccountAuthentication";
        private const String TwitterAuthenticationKey = "TwitterAuthentication";
        private const String FacebookAuthenticationKey = "FacebookAuthentication";
        private const String GoogleAuthenticationKey = "GoogleAuthentication";

        private readonly String _sendMailFrom;
        private readonly String _title;
        private readonly String _subTitle;
        private readonly String _authorName;
        private readonly String _authorMail;
        private readonly String _url;
        private readonly Int32 _postQuantityPerPage;
        private readonly Int32 _postQuantityPerSearch;
        private readonly Boolean _microsoftAccountAuthentication;
        private readonly Boolean _twitterAuthentication;
        private readonly Boolean _facebookAuthentication;
        private readonly Boolean _googleAuthentication;

        #endregion

        #region Constructors

        private Settings()
        {
            // mail
            _sendMailFrom = ConfigurationManager.AppSettings[SendMailFromKey];

            // blog configuration
            _url = GetFromConfiguration(UriKey, UriHelper.AbsoluteApplicationUri);
            _title = ConfigurationManager.AppSettings[TitleKey];
            _subTitle = ConfigurationManager.AppSettings[SubTitleKey];
            _authorName = ConfigurationManager.AppSettings[AuthorNameKey];
            _authorMail = ConfigurationManager.AppSettings[AuthorMailKey];

            // Display
            _postQuantityPerPage = GetFromConfiguration(PostQuantityPerPageKey, 5);
            _postQuantityPerSearch = GetFromConfiguration(PostQuantityPerSearchKey, 10);

            // Authentications
            _microsoftAccountAuthentication = GetFromConfiguration(MicrosoftAccountAuthenticationkey, false);
            _twitterAuthentication = GetFromConfiguration(TwitterAuthenticationKey, false);
            _facebookAuthentication = GetFromConfiguration(FacebookAuthenticationKey, false);
            _googleAuthentication = GetFromConfiguration(GoogleAuthenticationKey, false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Title
        /// </summary>
        public string Title => _title;

        /// <summary>
        /// Sub title
        /// </summary>
        public string SubTitle => _subTitle;

        /// <summary>
        /// Author name
        /// </summary>
        public string AuthorName => _authorName;

        /// <summary>
        /// Author mail
        /// </summary>
        public string AuthorMail => _authorMail;

        /// <summary>
        /// Author mail
        /// </summary>
        public string SendMailFrom => _sendMailFrom;

        /// <summary>
        /// Web site url
        /// </summary>
        public string Url => _url;

        /// <summary>
        /// Quantity of post available on each page
        /// </summary>
        public int PostQuantityPerPage => _postQuantityPerPage;

        /// <summary>
        /// Quantity of post available on each search
        /// </summary>
        public int PostQuantityPerSearch => _postQuantityPerSearch;

        /// <summary>
        /// Athentication with Microsoft Account is available
        /// </summary>
        public bool MicrosoftAccountAuthentication => _microsoftAccountAuthentication;

        /// <summary>
        /// Athentication with Twitter is available
        /// </summary>
        public bool TwitterAuthentication => _twitterAuthentication;

        /// <summary>
        /// Athentication with Facebook is available
        /// </summary>
        public bool FacebookAuthentication => _facebookAuthentication;

        /// <summary>
        /// Athentication with Google+ is available
        /// </summary>
        public bool GoogleAuthentication => _googleAuthentication;

        #endregion

        #region methodes

        /// <summary>
        /// Get String from configuration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static String GetFromConfiguration(String key, String defaultValue)
        {
            // Try to get setting
            String value = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrWhiteSpace(value)) return defaultValue;
            return value;
        }

        /// <summary>
        /// Get int from configuration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static Int32 GetFromConfiguration(String key, Int32 defaultValue)
        {
            // Try to get setting
            String value = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrWhiteSpace(value)) return defaultValue;

            // Try to convert setting in Int
            if (Int32.TryParse(value, out Int32 result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Get Boolean from configuration
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static Boolean GetFromConfiguration(String key, Boolean defaultValue)
        {
            // Try to get setting
            String value = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrWhiteSpace(value)) return defaultValue;

            // Try to convert setting in Int
            if (Boolean.TryParse(value, out Boolean result))
            {
                return result;
            }
            return defaultValue;
        }

        #endregion
    }
}