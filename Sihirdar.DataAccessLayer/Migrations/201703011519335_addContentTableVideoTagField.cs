using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addContentTableVideoTagField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "VideoUrlTag", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Content", "VideoUrlTag");
        }
    }
}
