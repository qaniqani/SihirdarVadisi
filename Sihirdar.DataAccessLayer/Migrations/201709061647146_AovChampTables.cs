namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AovChampTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AovChamp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        Key = c.String(),
                        Image = c.String(),
                        Role = c.String(),
                        Partype = c.String(),
                        Gold = c.Int(nullable: false),
                        Ticket = c.Int(nullable: false),
                        Lore = c.String(),
                        Quote = c.String(),
                        InfoDifficulty = c.Int(nullable: false),
                        InfoAttack = c.Int(nullable: false),
                        InfoConst = c.Int(nullable: false),
                        InfoMagic = c.Int(nullable: false),
                        Ad = c.Int(nullable: false),
                        Adperlevel = c.Int(nullable: false),
                        Ap = c.Int(nullable: false),
                        Apperlevel = c.Int(nullable: false),
                        Hp = c.Int(nullable: false),
                        Hpperlevel = c.Int(nullable: false),
                        Hpregen = c.Int(nullable: false),
                        Hpregenperlevel = c.Int(nullable: false),
                        Manaregen = c.Int(nullable: false),
                        Manaregenperlevel = c.Int(nullable: false),
                        Armor = c.Int(nullable: false),
                        ArmorPerLevel = c.Int(nullable: false),
                        Mr = c.Int(nullable: false),
                        MrPerLevel = c.Int(nullable: false),
                        As = c.Int(nullable: false),
                        AsPerLevel = c.Int(nullable: false),
                        Cd = c.Int(nullable: false),
                        CdPerLevel = c.Int(nullable: false),
                        Critic = c.Int(nullable: false),
                        CriticPerLevel = c.Int(nullable: false),
                        Movement = c.Int(nullable: false),
                        MovementPerLevel = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AovChampSkinAssng",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChampId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AovChampSpellAssng",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChampId = c.Int(nullable: false),
                        SpellId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AovSkin",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Num = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AovSpell",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Num = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AovSpell");
            DropTable("dbo.AovSkin");
            DropTable("dbo.AovChampSpellAssng");
            DropTable("dbo.AovChampSkinAssng");
            DropTable("dbo.AovChamp");
        }
    }
}
