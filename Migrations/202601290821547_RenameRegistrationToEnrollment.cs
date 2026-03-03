namespace KeeleSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameRegistrationToEnrollment : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Registrations", newName: "Enrollments");
            DropIndex("dbo.Enrollments", "IX_Training_User");
            AlterColumn("dbo.Enrollments", "Staatus", c => c.String(nullable: false));
            CreateIndex("dbo.Enrollments", "TrainingId");
            CreateIndex("dbo.Enrollments", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Enrollments", new[] { "ApplicationUserId" });
            DropIndex("dbo.Enrollments", new[] { "TrainingId" });
            AlterColumn("dbo.Enrollments", "Staatus", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Enrollments", new[] { "TrainingId", "ApplicationUserId" }, unique: true, name: "IX_Training_User");
            RenameTable(name: "dbo.Enrollments", newName: "Registrations");
        }
    }
}
