using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addCounterTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Counter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                        ContentUrl = c.String(),
                        ContentType = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Counter");
        }
    }
}
