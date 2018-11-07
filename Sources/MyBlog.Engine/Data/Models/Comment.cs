using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Data.Models
{
    public class Comment
    {
        #region Declarations
        #endregion

        #region Constructors
        #endregion

        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }

        [MaxLength(2000)]
        [Required]
        [DataType(DataType.MultilineText)]
        public String Text { get; set; }

        public DateTime DateCreatedGmt { get; set; }

        public virtual Post Post { get; set; }

        public virtual UserProfile Author { get; set; }

        #endregion

        #region Methods
        #endregion
    }
}
