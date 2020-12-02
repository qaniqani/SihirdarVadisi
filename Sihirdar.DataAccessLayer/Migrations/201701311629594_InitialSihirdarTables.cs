using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class InitialSihirdarTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "Tags", c => c.String());
            AddColumn("dbo.Content", "IsShowcase", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Content", "IsShowcase");
            DropColumn("dbo.Content", "Tags");
        }
    }
}
