namespace MyBlog.Engine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 40),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 150),
                        BeginningOfContent = c.String(),
                        EndOfContent = c.String(),
                        ContentIsSplitted = c.Boolean(nullable: false),
                        DateCreatedGmt = c.DateTime(nullable: false),
                        Published = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Publishers",
                c => new
                    {
                        Login = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Login);
            
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        Post_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Post_Id, t.Category_Id })
                .ForeignKey("dbo.Posts", t => t.Post_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Post_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.PostCategories", "Post_Id", "dbo.Posts");
            DropIndex("dbo.PostCategories", new[] { "Category_Id" });
            DropIndex("dbo.PostCategories", new[] { "Post_Id" });
            DropTable("dbo.PostCategories");
            DropTable("dbo.Publishers");
            DropTable("dbo.Posts");
            DropTable("dbo.Categories");
        }
    }
}
