using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MyLib.Web.Html.SocialExtensions;

namespace MyBlog.Models
{
    public sealed class ShareRequest
    {
        public String Id { get; set; }

        public SocialnetWork N { get; set; }
    }
}