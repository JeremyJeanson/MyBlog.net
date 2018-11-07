using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Engine.Data.Models
{
    public class PostLink
    {
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Url
        {
            get
            {
                return DataService.GetPostUrl(Id, Title);
            }
        }
    }
}
