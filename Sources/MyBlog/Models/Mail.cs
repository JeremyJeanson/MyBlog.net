using MyBlog.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Models
{
    public sealed class Mail
    {
        [Display(Name = "SenderName", ResourceType = typeof(Resources))]
        [MaxLength(100)]
        [Required]
        public String SenderName { get; set; }

        [Display(Name = "SenderMail", ResourceType = typeof(Resources))]
        [EmailAddress]
        [Required]
        public String SenderMail { get; set; }

        [Display(Name = "Subject", ResourceType = typeof(Resources))]
        [MaxLength(200)]
        [Required]
        public String Subject { get; set; }

        [Required]
        [AllowHtml(), DataType(DataType.MultilineText)]
        [MaxLength(2000)]
        public String Content { get; set; }
    }
}