using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class adminAddSocialFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admin", "AdminType", c => c.Int(nullable: false));
            AddColumn("dbo.Admin", "Picture", c => c.String());
            AddColumn("dbo.Admin", "Motto", c => c.String());
            AddColumn("dbo.Admin", "Facebook", c => c.String());
            AddColumn("dbo.Admin", "Twitter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admin", "Twitter");
            DropColumn("dbo.Admin", "Facebook");
            DropColumn("dbo.Admin", "Motto");
            DropColumn("dbo.Admin", "Picture");
            DropColumn("dbo.Admin", "AdminType");
        }
    }
}
