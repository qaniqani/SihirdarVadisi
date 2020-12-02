using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class falanlarFilanlar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Country");
        }
    }
}
