using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class sliderTableAddTypeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slider", "GameType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slider", "GameType");
        }
    }
}
