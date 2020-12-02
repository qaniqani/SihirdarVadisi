using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class userTableFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.User", "UpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "UpdatedDate");
            DropColumn("dbo.User", "CreatedDate");
        }
    }
}
