using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{ 
    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct CustomField
    {
        [XmlRpcMember("id")]
        public String Id;

        [XmlRpcMember("key")]
        public String Key;

        [XmlRpcMember("id")]
        public String Value;

    }
}