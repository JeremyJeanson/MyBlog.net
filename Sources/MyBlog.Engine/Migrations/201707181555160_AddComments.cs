namespace MyBlog.Engine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 2000),
                        DateCreatedGmt = c.DateTime(nullable: false),
                        Author_Id = c.Int(),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfiles", t => t.Author_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Issuer = c.String(maxLength: 15),
                        NameIdentifier = c.String(maxLength: 100),
                        Name = c.String(nullable: false, maxLength: 50),
                        Email = c.String(maxLength: 50),
                        EmailValidate = c.Boolean(nullable: false),
                        EmailValidationToken = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfilePosts",
                c => new
                    {
                        UserProfile_Id = c.Int(nullable: false),
                        Post_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserProfile_Id, t.Post_Id })
                .ForeignKey("dbo.UserProfiles", t => t.UserProfile_Id, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .Index(t => t.UserProfile_Id)
                .Index(t => t.Post_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.UserProfilePosts", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.UserProfilePosts", "UserProfile_Id", "dbo.UserProfiles");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.UserProfiles");
            DropIndex("dbo.UserProfilePosts", new[] { "Post_Id" });
            DropIndex("dbo.UserProfilePosts", new[] { "UserProfile_Id" });
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropTable("dbo.UserProfilePosts");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.Comments");
        }
    }
}
