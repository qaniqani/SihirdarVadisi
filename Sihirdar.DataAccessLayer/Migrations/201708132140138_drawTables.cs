namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drawTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DrawDefinition",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        ApiKey = c.String(),
                        Name = c.String(),
                        Detail = c.String(),
                        WinCount = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DrawMember",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApiKey = c.String(),
                        Title = c.String(),
                        Name = c.String(),
                        Surname = c.String(),
                        SiteUrl = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Gsm = c.String(),
                        ProjectDetail = c.String(),
                        StatusType = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DrawUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MemberId = c.Int(nullable: false),
                        DrawId = c.Int(nullable: false),
                        UserGuid = c.String(),
                        ApiKey = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Win = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DrawUser");
            DropTable("dbo.DrawMember");
            DropTable("dbo.DrawDefinition");
        }
    }
}
