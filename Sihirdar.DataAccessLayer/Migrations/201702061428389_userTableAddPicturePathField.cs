using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class userTableAddPicturePathField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "PicturePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "PicturePath");
        }
    }
}
