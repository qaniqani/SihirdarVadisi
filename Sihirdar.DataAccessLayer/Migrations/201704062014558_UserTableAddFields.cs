using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class UserTableAddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "City", c => c.String());
            AddColumn("dbo.User", "BirthDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "BirthDate");
            DropColumn("dbo.User", "City");
        }
    }
}
