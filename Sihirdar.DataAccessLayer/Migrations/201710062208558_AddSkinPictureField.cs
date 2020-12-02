namespace Sihirdar.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSkinPictureField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AovSkin", "Picture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AovSkin", "Picture");
        }
    }
}
