using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class userTableAddBannedMessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "BannedMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "BannedMessage");
        }
    }
}
