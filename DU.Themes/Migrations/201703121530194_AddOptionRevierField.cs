namespace DU.Themes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOptionRevierField : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Themes", "ReviewerId", "dbo.Users");
            DropIndex("dbo.Themes", new[] { "ReviewerId" });
            AlterColumn("dbo.Themes", "ReviewerId", c => c.Long());
            CreateIndex("dbo.Themes", "ReviewerId");
            AddForeignKey("dbo.Themes", "ReviewerId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "ReviewerId", "dbo.Users");
            DropIndex("dbo.Themes", new[] { "ReviewerId" });
            AlterColumn("dbo.Themes", "ReviewerId", c => c.Long(nullable: false));
            CreateIndex("dbo.Themes", "ReviewerId");
            AddForeignKey("dbo.Themes", "ReviewerId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
