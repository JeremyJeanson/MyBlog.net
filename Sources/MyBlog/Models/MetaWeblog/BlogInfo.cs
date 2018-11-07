using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct BlogInfo
    {
        [XmlRpcMember("blogid")]
        public String Id;

        [XmlRpcMember("url")]
        public String Url;

        [XmlRpcMember("blogName")]
        public String BlogName;

        [XmlRpcMember("isAdmin")]
        public Boolean IsAdmin;

        [XmlRpcMember("xmlrpc")]
        public String XmlRpc;
    }
}