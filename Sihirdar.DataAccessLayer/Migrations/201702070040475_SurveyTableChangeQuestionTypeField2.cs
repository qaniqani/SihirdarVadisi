using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class SurveyTableChangeQuestionTypeField2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Survey", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Survey", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Survey", "EndDate", c => c.String());
            AlterColumn("dbo.Survey", "StartDate", c => c.String());
        }
    }
}
