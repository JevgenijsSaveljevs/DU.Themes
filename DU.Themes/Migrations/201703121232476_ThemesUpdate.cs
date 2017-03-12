namespace DU.Themes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ThemesUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Themes", "WorkEnd_Id", "dbo.StudyYears");
            DropIndex("dbo.Themes", new[] { "WorkEnd_Id" });
            RenameColumn(table: "dbo.Themes", name: "WorkEnd_Id", newName: "EndId");
            RenameColumn(table: "dbo.Themes", name: "WorkStartId", newName: "StartId");
            RenameIndex(table: "dbo.Themes", name: "IX_WorkStartId", newName: "IX_StartId");
            AddColumn("dbo.Themes", "ThemeENG", c => c.String());
            AlterColumn("dbo.Themes", "EndId", c => c.Long(nullable: false));
            CreateIndex("dbo.Themes", "EndId");
            AddForeignKey("dbo.Themes", "EndId", "dbo.StudyYears", "Id", cascadeDelete: true);
            DropColumn("dbo.Themes", "ThemeRU");
            DropColumn("dbo.Themes", "SeenByReviewer");
            DropColumn("dbo.Themes", "WorkStartEndId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Themes", "WorkStartEndId", c => c.Long(nullable: false));
            AddColumn("dbo.Themes", "SeenByReviewer", c => c.Boolean(nullable: false));
            AddColumn("dbo.Themes", "ThemeRU", c => c.String());
            DropForeignKey("dbo.Themes", "EndId", "dbo.StudyYears");
            DropIndex("dbo.Themes", new[] { "EndId" });
            AlterColumn("dbo.Themes", "EndId", c => c.Long());
            DropColumn("dbo.Themes", "ThemeENG");
            RenameIndex(table: "dbo.Themes", name: "IX_StartId", newName: "IX_WorkStartId");
            RenameColumn(table: "dbo.Themes", name: "StartId", newName: "WorkStartId");
            RenameColumn(table: "dbo.Themes", name: "EndId", newName: "WorkEnd_Id");
            CreateIndex("dbo.Themes", "WorkEnd_Id");
            AddForeignKey("dbo.Themes", "WorkEnd_Id", "dbo.StudyYears", "Id");
        }
    }
}
