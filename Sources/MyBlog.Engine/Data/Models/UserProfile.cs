using MyBlog.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBlog.Engine.Data.Models
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [MaxLength(15)]
        public String Issuer { get; set; }

        [MaxLength(100)]
        public String NameIdentifier { get; set; }

        [MaxLength(50)]
        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources))]
        public String Name { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(Resources))]
        public String Email { get; set; }

        public Boolean EmailValidate { get; set; }

        public Guid EmailValidationToken { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Post> CommentsFollowed { get; set; }
    }
}
