namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTournamentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserTeam",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GameName = c.String(),
                        Username1 = c.String(),
                        Username2 = c.String(),
                        Username3 = c.String(),
                        Username4 = c.String(),
                        Username5 = c.String(),
                        UserNick1 = c.String(),
                        UserNick2 = c.String(),
                        UserNick3 = c.String(),
                        UserNick4 = c.String(),
                        UserNick5 = c.String(),
                        BackupUsername1 = c.String(),
                        BackupUsername2 = c.String(),
                        BackupUserNick1 = c.String(),
                        BackupUserNick2 = c.String(),
                        Phone = c.String(),
                        CreteDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
        }
        
        public override void Down()
        {
            DropTable("dbo.UserTeam");
        }
    }
}
