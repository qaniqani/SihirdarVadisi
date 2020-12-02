using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class contentTableChangeFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "VideoUrl", c => c.String());
            DropColumn("dbo.Content", "EmbeddedCode");
            DropColumn("dbo.Content", "VideoUrlTag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Content", "VideoUrlTag", c => c.String());
            AddColumn("dbo.Content", "EmbeddedCode", c => c.String());
            DropColumn("dbo.Content", "VideoUrl");
        }
    }
}
