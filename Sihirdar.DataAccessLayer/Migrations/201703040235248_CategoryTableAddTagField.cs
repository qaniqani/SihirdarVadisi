using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class CategoryTableAddTagField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "CategoryTagType", c => c.Int(nullable: false));
            AddColumn("dbo.UserVoteAssgn", "SurveyId", c => c.Int(nullable: false));
            DropColumn("dbo.User", "BirthDate");
            DropColumn("dbo.User", "Country");
            DropColumn("dbo.User", "City");
            DropColumn("dbo.User", "Region");
            DropColumn("dbo.User", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Address", c => c.String());
            AddColumn("dbo.User", "Region", c => c.String());
            AddColumn("dbo.User", "City", c => c.String());
            AddColumn("dbo.User", "Country", c => c.String());
            AddColumn("dbo.User", "BirthDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserVoteAssgn", "SurveyId");
            DropColumn("dbo.Category", "CategoryTagType");
        }
    }
}
