namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserFbField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "FacebookDetail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "FacebookDetail");
        }
    }
}
