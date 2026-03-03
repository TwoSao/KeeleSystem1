namespace KeeleSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameRegistrationToEnrollment1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Enrollments", new[] { "TrainingId" });
            DropIndex("dbo.Enrollments", new[] { "ApplicationUserId" });
            AddColumn("dbo.Enrollments", "Status", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Enrollments", new[] { "TrainingId", "ApplicationUserId" }, unique: true, name: "IX_Training_User");
            DropColumn("dbo.Enrollments", "Staatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Enrollments", "Staatus", c => c.String(nullable: false));
            DropIndex("dbo.Enrollments", "IX_Training_User");
            DropColumn("dbo.Enrollments", "Status");
            CreateIndex("dbo.Enrollments", "ApplicationUserId");
            CreateIndex("dbo.Enrollments", "TrainingId");
        }
    }
}
