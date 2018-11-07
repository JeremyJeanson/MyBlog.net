using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{
#pragma warning disable 649

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    internal struct CategoryInfo
    {
        [XmlRpcMember("categoryId")]
        public String Id;

        // Not  used
        [XmlRpcMember("parentId")]
        public String ParentId;

        [XmlRpcMember("categoryName")]
        public String Name;

        [XmlRpcMember("categoryDescription")]
        public String CategoryDescription;

        [XmlRpcMember("Description")]
        public String Description;

        [XmlRpcMember("htmlUrl")]
        public String Url;

        [XmlRpcMember("rssUrl")]
        public String Rss;
    }
#pragma warning restore 649
}