using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class AdminTableAddFieldAuthorization : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admin", "Authorization", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admin", "Authorization");
        }
    }
}
