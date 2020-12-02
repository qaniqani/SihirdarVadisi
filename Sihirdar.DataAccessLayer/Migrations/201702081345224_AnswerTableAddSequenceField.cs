using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class AnswerTableAddSequenceField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answer", "SequenceNumber", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answer", "SequenceNumber");
        }
    }
}
