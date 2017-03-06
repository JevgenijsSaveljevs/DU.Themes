namespace DU.Themes.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StudentId = c.Long(nullable: false),
                        TeacherId = c.Long(nullable: false),
                        ReviewerId = c.Long(),
                        ThemeLV = c.String(),
                        ThemeENG = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RespondedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SeenByTeacher = c.Boolean(nullable: false),
                        SeenByStudent = c.Boolean(nullable: false),
                        ThemeAccepted = c.Boolean(nullable: false),
                        SupervisorAccpeted = c.Boolean(nullable: false),
                        TouchTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReviewerId)
                .ForeignKey("dbo.Users", t => t.StudentId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.TeacherId, cascadeDelete: false)
                .Index(t => t.StudentId)
                .Index(t => t.TeacherId)
                .Index(t => t.ReviewerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.String(),
                        FirstName = c.String(maxLength: 150),
                        LastName = c.String(maxLength: 150),
                        StudentIdentifier = c.String(),
                        ProgramName = c.String(maxLength: 150),
                        ProgramLevel = c.String(),
                        StudyForm = c.String(),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        Person_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Long(nullable: false),
                        Person_Id = c.Long(),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudyYears",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        End = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Code = c.String(maxLength: 20),
                        Name = c.String(maxLength: 150),
                        TouchTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Themes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StudentId = c.Long(nullable: false),
                        TeacherId = c.Long(nullable: false),
                        ReviewerId = c.Long(nullable: false),
                        ThemeLV = c.String(),
                        ThemeRU = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RespondedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SeenByTeacher = c.Boolean(nullable: false),
                        SeenByReviewer = c.Boolean(nullable: false),
                        SeenByStudent = c.Boolean(nullable: false),
                        WorkStartId = c.Long(nullable: false),
                        WorkStartEndId = c.Long(nullable: false),
                        TouchTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        WorkEnd_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReviewerId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.StudentId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.TeacherId, cascadeDelete: false)
                .ForeignKey("dbo.StudyYears", t => t.WorkEnd_Id)
                .ForeignKey("dbo.StudyYears", t => t.WorkStartId, cascadeDelete: false)
                .Index(t => t.StudentId)
                .Index(t => t.TeacherId)
                .Index(t => t.ReviewerId)
                .Index(t => t.WorkStartId)
                .Index(t => t.WorkEnd_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Themes", "WorkStartId", "dbo.StudyYears");
            DropForeignKey("dbo.Themes", "WorkEnd_Id", "dbo.StudyYears");
            DropForeignKey("dbo.Themes", "TeacherId", "dbo.Users");
            DropForeignKey("dbo.Themes", "StudentId", "dbo.Users");
            DropForeignKey("dbo.Themes", "ReviewerId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Requests", "TeacherId", "dbo.Users");
            DropForeignKey("dbo.Requests", "StudentId", "dbo.Users");
            DropForeignKey("dbo.Requests", "ReviewerId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "Person_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserClaims", "Person_Id", "dbo.Users");
            DropIndex("dbo.Themes", new[] { "WorkEnd_Id" });
            DropIndex("dbo.Themes", new[] { "WorkStartId" });
            DropIndex("dbo.Themes", new[] { "ReviewerId" });
            DropIndex("dbo.Themes", new[] { "TeacherId" });
            DropIndex("dbo.Themes", new[] { "StudentId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "Person_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "Person_Id" });
            DropIndex("dbo.Requests", new[] { "ReviewerId" });
            DropIndex("dbo.Requests", new[] { "TeacherId" });
            DropIndex("dbo.Requests", new[] { "StudentId" });
            DropTable("dbo.Themes");
            DropTable("dbo.StudyYears");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Requests");
        }
    }
}
