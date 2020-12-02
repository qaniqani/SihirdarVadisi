using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addLiveBroadcastTableFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiveBroadcast", "LanguageId", c => c.Int(nullable: false));
            AddColumn("dbo.LiveBroadcast", "Language", c => c.String());
            AddColumn("dbo.LiveBroadcast", "SequenceNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LiveBroadcast", "SequenceNumber");
            DropColumn("dbo.LiveBroadcast", "Language");
            DropColumn("dbo.LiveBroadcast", "LanguageId");
        }
    }
}
