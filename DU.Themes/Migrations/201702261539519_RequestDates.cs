namespace DU.Themes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "StartId", c => c.Long(nullable: false));
            AddColumn("dbo.Requests", "EndId", c => c.Long(nullable: false));
            CreateIndex("dbo.Requests", "StartId");
            CreateIndex("dbo.Requests", "EndId");
            AddForeignKey("dbo.Requests", "EndId", "dbo.StudyYears", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Requests", "StartId", "dbo.StudyYears", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "StartId", "dbo.StudyYears");
            DropForeignKey("dbo.Requests", "EndId", "dbo.StudyYears");
            DropIndex("dbo.Requests", new[] { "EndId" });
            DropIndex("dbo.Requests", new[] { "StartId" });
            DropColumn("dbo.Requests", "EndId");
            DropColumn("dbo.Requests", "StartId");
        }
    }
}
