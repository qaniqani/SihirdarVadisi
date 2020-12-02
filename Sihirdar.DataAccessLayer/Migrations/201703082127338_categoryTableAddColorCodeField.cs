using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class categoryTableAddColorCodeField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "ColorCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "ColorCode");
        }
    }
}
