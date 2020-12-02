using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addTagsField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gallery", "Tags", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gallery", "Tags");
        }
    }
}
