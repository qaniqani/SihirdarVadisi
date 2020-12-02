namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArenaValorChampTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArenaValorChamp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Detail = c.String(),
                        Picture = c.String(),
                        Url = c.String(),
                        Status = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ArenaValorChamp");
        }
    }
}
