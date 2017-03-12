namespace DU.Themes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThemeActiveField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Themes", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Themes", "Active");
        }
    }
}
