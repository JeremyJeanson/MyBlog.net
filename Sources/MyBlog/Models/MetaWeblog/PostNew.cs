using CookComputing.XmlRpc;
using System;

namespace MyBlog.Models.MetaWeblog
{
#pragma warning disable 649

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    internal struct PostNew
    {
        [XmlRpcMember("title")]
        public String Title;

        [XmlRpcMember("description")]
        public String Description;

        [XmlRpcMember("post_type")]
        public String Type;

        [XmlRpcMember("date_created")]
        public Nullable<DateTime> DateCreated;

        [XmlRpcMember("date_created_gmt")]
        public Nullable<DateTime> DateCreatedGmt;

        [XmlRpcMember("categories")]
        public String[] Categories;

        [XmlRpcMember("mt_keywords")]
        public String[] KeyWords;

        [XmlRpcMember("mt_excerpt")]
        public String MtExcerpt;

        [XmlRpcMember("mt_text_more")]
        public String MtTextMore;

        [XmlRpcMember("mt_allow_comments")]
        public String MtAllowComments;

        [XmlRpcMember("mt_allow_pings")]
        public String MtAllowPings;

        [XmlRpcMember("wp_slug")]
        public String WpSlug;

        [XmlRpcMember("wp_password")]
        public String WpPassword;

        [XmlRpcMember("wp_author_id")]
        public String WpAuthorId;

        [XmlRpcMember("wp_author_display_name")]
        public String WpAuthorDisplayName;

        [XmlRpcMember("post_status")]
        public String Status;

        [XmlRpcMember("wp_post_format")]
        public String WpFormat;

        [XmlRpcMember("sticky")]
        public Boolean Sticky;

        [XmlRpcMember("custom_fields")]
        public CustomField[] CustomFields;

        [XmlRpcMember("enclosure")]
        public Enclosure Enclosure;
    }

#pragma warning restore 649
}