using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        [XmlRpcMember("url")]
        public String Url;

        [XmlRpcMember("length")]
        public Int32 Length;

        [XmlRpcMember("type")]
        public String Type;
    }
}