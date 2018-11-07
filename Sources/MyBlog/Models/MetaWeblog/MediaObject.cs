using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{
#pragma warning disable 649

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    internal struct MediaObject
    {
        [XmlRpcMember("name")]
        public String Name;

        [XmlRpcMember("type")]
        public String Type;

        [XmlRpcMember("bits")]
        public Byte[] Bits;
    }

#pragma warning restore 649
}