using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class AdvertTableAddCategoryIdField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Advert", "CategoryId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Advert", "CategoryId");
        }
    }
}
