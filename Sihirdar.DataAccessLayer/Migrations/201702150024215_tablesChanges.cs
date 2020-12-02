using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class tablesChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Content", "EmbeddedCode", c => c.String());
            DropTable("dbo.Video");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Video",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        Subject = c.String(),
                        Keyword = c.String(),
                        Description = c.String(),
                        EmbedCode = c.String(),
                        Tags = c.String(),
                        Url = c.String(),
                        Status = c.Int(nullable: false),
                        CreateEditorId = c.Int(nullable: false),
                        UpdateEditorId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Content", "EmbeddedCode");
        }
    }
}
