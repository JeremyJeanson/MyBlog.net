using System;
using System.Collections.Generic;

namespace MyBlog.Engine.Data.Models
{
    public sealed class PostWithSummary: PostDesplayedBase
    {
        public String Summary { get; set; }

        public Boolean ContentIsSplitted { get; set; }

        public String HtmlSummary
        {
            get
            {
                return Summary;
                // CommonMark.CommonMarkConverter.Convert(Summary);
            }
        }
    }
}
