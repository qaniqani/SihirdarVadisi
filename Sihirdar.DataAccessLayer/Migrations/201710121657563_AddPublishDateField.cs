namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPublishDateField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "PublishDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Content", "PublishDate");
        }
    }
}
