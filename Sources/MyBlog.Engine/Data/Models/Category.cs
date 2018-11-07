using MyLib.Web.Helpers;
using MyLib.Web.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Engine.Data.Models
{
    public class Category
    {
        #region Déclarations

        private const String UrlFormat = "{0}/Post/Category/{1}/{2}/";

        #endregion

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [MaxLength(40)]
        public String Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        #region Genenrated properties

        /// <summary>
        /// Url
        /// </summary>
        public String Url
        {
            get { return GetUrl(Id, Name); }
        }

        /// <summary>
        /// Return Url for category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetUrl(int id, string name)
        {
            return String.Format(
                UrlFormat,
                Settings.Current.Url,
                id.ToString(),
                UriHelper.ToFriendly(name));
        }


        #endregion
    }
}