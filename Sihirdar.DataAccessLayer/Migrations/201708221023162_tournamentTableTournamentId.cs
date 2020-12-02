namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tournamentTableTournamentId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTeam", "TournamentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTeam", "TournamentId");
        }
    }
}
