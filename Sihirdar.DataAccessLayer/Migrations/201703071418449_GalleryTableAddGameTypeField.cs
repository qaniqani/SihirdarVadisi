using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class GalleryTableAddGameTypeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gallery", "GameType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gallery", "GameType");
        }
    }
}
