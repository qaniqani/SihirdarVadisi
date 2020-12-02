using System.Data.Entity.Migrations;

namespace Sihirdar.DataAccessLayer.Migrations
{
    public partial class AddPictureSizeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advert",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Name = c.String(),
                        AdGuid = c.String(),
                        AdLocation = c.Int(nullable: false),
                        AdType = c.Int(nullable: false),
                        AdCode = c.String(),
                        AdLink = c.String(),
                        AdFileType = c.String(),
                        AdFilePath = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        ViewHit = c.Int(nullable: false),
                        ClickHit = c.Int(nullable: false),
                        SequenceNr = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        CreateEditorId = c.Int(nullable: false),
                        UpdateEditorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        Response = c.String(),
                        Vote = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Picture",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureType = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                        SizeId = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        PicturePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PictureSizeDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SizeId = c.Int(nullable: false),
                        Name = c.String(),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PictureSize",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureType = c.Int(nullable: false),
                        Name = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PromiseDay",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(),
                        LanguageId = c.Int(nullable: false),
                        Promise = c.String(),
                        Teller = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        CreateEditorId = c.Int(nullable: false),
                        UpdateEditorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Survey",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LanguageId = c.Int(nullable: false),
                        Language = c.String(),
                        Question = c.String(),
                        QuestionType = c.String(),
                        StartDate = c.String(),
                        EndDate = c.String(),
                        Status = c.Int(nullable: false),
                        CreateEditorId = c.Int(nullable: false),
                        UpdateEditorId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefaultLanguage = c.String(),
                        DefaultLanguageId = c.Int(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Email = c.String(),
                        Gsm = c.String(),
                        InterestAreas = c.String(),
                        Gender = c.Int(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        Address = c.String(),
                        Password = c.String(),
                        ActivationCode = c.String(),
                        SignInType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        Spot = c.String(),
                        EmbedCode = c.String(),
                        Status = c.Int(nullable: false),
                        CreateEditorId = c.String(),
                        UpdateEditorId = c.String(),
                        CreatedDate = c.String(),
                        UpdatedDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Video");
            DropTable("dbo.User");
            DropTable("dbo.Survey");
            DropTable("dbo.PromiseDay");
            DropTable("dbo.PictureSize");
            DropTable("dbo.PictureSizeDetail");
            DropTable("dbo.Picture");
            DropTable("dbo.Answer");
            DropTable("dbo.Advert");
        }
    }
}
