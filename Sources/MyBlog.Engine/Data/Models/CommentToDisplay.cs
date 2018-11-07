﻿using System;

namespace MyBlog.Engine.Data.Models
{
    public sealed class CommentToDisplay
    {
        public String Text { get; set; }

        public String Html
        {
            get
            {
                return Text;
                    //CommonMark.CommonMarkConverter.Convert(Text);
            }
        }

        public DateTime DateCreatedGmt { get; set; }

        public String Author { get; set; }
    }
}
