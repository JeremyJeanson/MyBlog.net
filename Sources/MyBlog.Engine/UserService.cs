using Microsoft.Owin.Security;
using MyBlog.Engine.Data.Models;
using System;
using System.Security.Claims;
using System.Web;

namespace MyBlog.Engine
{
    public static class UserService
    {
        #region Declarations

        private const String UserProfileSessionKey = "UserProfile";

        #endregion

        #region Constructors
        #endregion

        #region Properties
        #endregion

        #region Methods

        private static IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        /// <summary>
        /// Get user from claims
        /// </summary>
        /// <returns></returns>
        public static UserProfile GetFromClaims()
        {
            using (var db = new DataService())
            {
                return GetFromClaims(db);
            }
        }

        /// <summary>
        /// Get user from claims
        /// </summary>
        /// <returns></returns>
        public static UserProfile GetFromClaims(DataService db)
        {
            ClaimsPrincipal principal = AuthenticationManager.User;
            if (principal == null) return null;

            // Get Identifier
            Claim nameIdentifierClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            // Get name
            Claim nameClaim = principal.FindFirst(ClaimTypes.Name);
            // Add mail
            Claim mailClaim = principal.FindFirst(ClaimTypes.Email);

            // Test nameIdentifier
            if (nameIdentifierClaim == null) return null;

            // Get values
            String issuer = nameIdentifierClaim.Issuer;
            String nameIdentifier = nameIdentifierClaim.Value;

            // Try to get User from database

            // Try to get user from database
            UserProfile user = db.GetUser(issuer, nameIdentifier);

            // Test user
            if (user == null)
            {
                user = new UserProfile
                {
                    Issuer = issuer,
                    NameIdentifier = nameIdentifier,
                    Name = nameClaim?.Value,
                    Email = mailClaim?.Value,
                    EmailValidate = false
                };
            }

            // Return the user
            return user;
        }

        /// <summary>
        /// Get user from session 
        /// </summary>
        /// <returns></returns>
        public static UserProfile Get()
        {
            return Get(null);
        }

        /// <summary>
        /// Get fresh useer updated from dataBase for edition
        /// </summary>
        /// <returns></returns>
        public static UserProfile GetFreshUseerUpdatedFromDataBase()
        {
            using (var db = new DataService())
            {
                // Try know user id from session
                UserProfile user = HttpContext.Current.Session[UserProfileSessionKey] as UserProfile;

                // Try to get user from claims
                if (user == null && AuthenticationManager.User != null)
                {
                    user = GetFromClaims(db);
                }
                if (user == null) return null;

                // Get fresh data from data base if user is not new
                if (user.Id != 0)
                {
                    user = db.GetUser(user.Id);
                }

                // return user
                return user;
            }
        }

        /// <summary>
        /// Get user from session
        /// </summary>
        /// <returns></returns>
        public static UserProfile Get(DataService db)
        {
            // Try to get user from session
            UserProfile user = HttpContext.Current.Session[UserProfileSessionKey] as UserProfile;

            // Try to get user from claims
            if (user == null && AuthenticationManager.User != null)
            {
                Boolean localDb = db == null;
                if (localDb) db = new DataService();
                user = GetFromClaims(db);
                if (localDb) db.Dispose();
                HttpContext.Current.Session[UserProfileSessionKey] = user;
            }

            // return user
            return user;
        }

        /// <summary>
        /// Set user in session
        /// </summary>
        /// <param name="user"></param>
        public static void Set(UserProfile user)
        {
            HttpContext.Current.Session[UserProfileSessionKey] = user;
        }

        /// <summary>
        /// Set user in session
        /// </summary>
        /// <param name="user"></param>
        public static void Clear()
        {
            HttpContext.Current.Session[UserProfileSessionKey] = null;
            HttpContext.Current.Session.Abandon();
        }

        #endregion
    }
}
