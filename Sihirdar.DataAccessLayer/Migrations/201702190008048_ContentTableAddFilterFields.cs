using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class ContentTableAddFilterFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "GameType", c => c.Int(nullable: false));
            AddColumn("dbo.Content", "FilterType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Content", "FilterType");
            DropColumn("dbo.Content", "GameType");
        }
    }
}
