namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teamSaveTeamNAme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserTeam", "TeamName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserTeam", "TeamName");
        }
    }
}
