namespace DU.Themes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsCurrentPropertyForStudyYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudyYears", "IsCurrent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudyYears", "IsCurrent");
        }
    }
}
