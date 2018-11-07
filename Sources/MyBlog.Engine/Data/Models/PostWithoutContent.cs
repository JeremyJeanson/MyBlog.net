using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Data.Models
{
    public sealed class PostWithoutContent
    {
        public Int32 Id { get; set; }

        public String Title { get; set; }

        public DateTime DateCreatedGmt { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public String Url
        {
            get
            {
                return DataService.GetPostUrl(Id, Title);
            }
        }
    }
}
