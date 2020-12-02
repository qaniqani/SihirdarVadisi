using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class videoTableChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Video", "Tags", c => c.String());
            AddColumn("dbo.Video", "Url", c => c.String());
            AlterColumn("dbo.Video", "CreateEditorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Video", "UpdateEditorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Video", "CreatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Video", "UpdatedDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Video", "Spot");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Video", "Spot", c => c.String());
            AlterColumn("dbo.Video", "UpdatedDate", c => c.String());
            AlterColumn("dbo.Video", "CreatedDate", c => c.String());
            AlterColumn("dbo.Video", "UpdateEditorId", c => c.String());
            AlterColumn("dbo.Video", "CreateEditorId", c => c.String());
            DropColumn("dbo.Video", "Url");
            DropColumn("dbo.Video", "Tags");
        }
    }
}
