using System;
using System.Web;
using System.Web.Optimization;

namespace MyBlog.Engine
{
    public sealed class UserSettingsService
    {
        #region Declarations

        private const String KeyName = "UserSettings";
        private const String CookiesConcentClosedName = "CookiesConcentClosed";
        private const String UseDyslexicFontName = "UseDyslexicFont";

        #endregion

        #region Methodes

        /// <summary>
        /// Get current user's settings
        /// </summary>
        /// <returns></returns>
        public static UserSettings Get()
        {
            // get settings from session
            // & Test settings
            if (!(HttpContext.Current.Session[KeyName] is UserSettings settings))
            {
                // Get settings from cookie
                settings = GetFromCookie();
                // Store current settings in session
                HttpContext.Current.Session[KeyName] = settings;
            }
            return settings;
        }

        /// <summary>
        /// Set current user's settings
        /// </summary>
        /// <param name="settings"></param>
        public static void Set(UserSettings settings)
        {
            // Save session
            HttpContext.Current.Session[KeyName] = settings;

            // Update or delate the cookie
            if (settings == null)
            {
                DeleteCookie();
            }
            else
            {
                SaveCookie(settings);
            }
        }

        /// <summary>
        /// Get user's settings from th cookie
        /// </summary>
        /// <returns></returns>
        private static UserSettings GetFromCookie()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[KeyName];
            // Test the cookie
            if (cookie == null)
            {
                // Default values
                return new UserSettings
                {
                    CookiesConcentClosed = false,
                    UseDyslexicFont = false
                };
            }
            else
            {
                // Get values from the cookie
                return new UserSettings
                {
                    CookiesConcentClosed = GetValueFromCookie(cookie, CookiesConcentClosedName, false),
                    UseDyslexicFont = GetValueFromCookie(cookie, UseDyslexicFontName, false),
                };
            }
        }


        /// <summary>
        /// Get a booleran value form the cookie
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private static Boolean GetValueFromCookie(HttpCookie cookie, String key, Boolean defaultValue)
        {
            // Read value
            String value = cookie[key];

            // Value no found
            if (String.IsNullOrEmpty(value)) return defaultValue;

            // Try to get the result
            if (Boolean.TryParse(value, out Boolean result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// Save settings into the cookie
        /// </summary>
        /// <param name="settings"></param>
        private static void SaveCookie(UserSettings settings)
        {
            // Get the current cookie or a new cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies[KeyName] ?? new HttpCookie(KeyName) { Expires = DateTime.Now.AddYears(1) };

            // Set values
            cookie[CookiesConcentClosedName] = settings.CookiesConcentClosed.ToString();
            cookie[UseDyslexicFontName] = settings.UseDyslexicFont.ToString();

            // update or add the cookie
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// Delete the current cookie
        /// </summary>
        private static void DeleteCookie()
        {
            // Creat the cookie do delete via the user's browser
            HttpCookie cookie = new HttpCookie(KeyName)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        #endregion
    }

    public sealed class UserSettings
    {
        #region Declarations

        private Boolean _useDyslexicFont;
        private string _layoutContentUrl;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal UserSettings()
        {

        }

        #endregion

        #region Properties

        public Boolean CookiesConcentClosed { get; set; }
        public Boolean UseDyslexicFont { get => _useDyslexicFont; set { _useDyslexicFont = value; _layoutContentUrl = GetContentLayoutUrl(); } }
        public String LayoutContentUrl { get => _layoutContentUrl ?? (_layoutContentUrl = GetContentLayoutUrl()); set => _layoutContentUrl = value; }

        #endregion

        #region Methodes

        /// <summary>
        /// Get Url for the CSS used by layout for this user
        /// </summary>
        /// <returns></returns>
        private String GetContentLayoutUrl()
        {
            return BundleTable.Bundles.ResolveBundleUrl(_useDyslexicFont
                ? Urls.ContentDys
                : Urls.ContentDefault);
        }

        #endregion
    }
}
