namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class skidAddColStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AovSkin", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AovSkin", "Status");
        }
    }
}
