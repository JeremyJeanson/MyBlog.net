using MyBlog.Engine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models
{
    public sealed class AccountProviders
    {
        public String ReturnUrl { get; set; }
        public AccountProvider[] Providers { get; set; }
    }

    public sealed class AccountProvider
    {
        public String Style { get; set; }
        public String Icon { get; set; }
        public String Name { get; set; }
        public String Provider { get; set; }
    }

    public sealed class EditUserProfile
    {
        public UserProfile User { get; set; }

        public Nullable<Boolean> Success { get; set; }
    }
}