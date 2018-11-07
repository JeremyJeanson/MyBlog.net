namespace MyBlog.Engine.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HashPublisherPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Publishers", "Salt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Publishers", "Salt");
        }
    }
}
