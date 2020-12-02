using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class AddTableUserVoteAssgn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserVoteAssgn",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        AnswerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserVoteAssgn");
        }
    }
}
