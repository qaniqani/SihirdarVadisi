using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class answerTableAddSurveyIdField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "SurveyId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answer", "SurveyId");
        }
    }
}
