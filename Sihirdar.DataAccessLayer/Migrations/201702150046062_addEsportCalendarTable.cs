using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class addEsportCalendarTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EsportCalendar",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        Name = c.String(),
                        Picture = c.String(),
                        StartDateTime = c.DateTime(nullable: false),
                        Color = c.String(),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedEditorId = c.Int(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UpdatedEditorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EsportCalendar");
        }
    }
}
