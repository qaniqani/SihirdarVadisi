using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addLiveBroadcastTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LiveBroadcast",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameType = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        PublishAddress = c.String(),
                        ChatAddress = c.String(),
                        Live = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LiveBroadcast");
        }
    }
}
