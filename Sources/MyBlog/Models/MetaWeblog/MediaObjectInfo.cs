using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    internal struct MediaObjectInfo
    {
        [XmlRpcMember("id")]
        public String Id;

        [XmlRpcMember("file")]
        public String Name;

        [XmlRpcMember("url")]
        public String Url;

        [XmlRpcMember("type")]
        public String Type;
    }
}