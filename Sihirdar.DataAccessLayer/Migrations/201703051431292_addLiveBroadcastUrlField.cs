using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addLiveBroadcastUrlField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiveBroadcast", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LiveBroadcast", "Url");
        }
    }
}
