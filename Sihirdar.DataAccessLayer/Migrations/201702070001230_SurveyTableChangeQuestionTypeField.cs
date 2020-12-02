using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class SurveyTableChangeQuestionTypeField : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Survey", "QuestionType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Survey", "QuestionType", c => c.String());
        }
    }
}
