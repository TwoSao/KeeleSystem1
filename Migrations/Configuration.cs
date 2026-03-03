namespace KeeleSystem.Migrations
{
    using KeeleSystem.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration
        : DbMigrationsConfiguration<KeeleSystem.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            // =========================
            // ROLE MANAGER
            // =========================
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            string[] roles = { "Admin", "Opetaja", "Opilane" };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new IdentityRole(role));
                }
            }

            // =========================
            // USER MANAGER
            // =========================
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            string adminEmail = "admin@test.ee";
            string teacherEmail = "teacher@test.ee";
            string password = "Test123!";

            // =========================
            // ADMIN USER
            // =========================
            var adminUser = userManager.FindByEmail(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                userManager.Create(adminUser, password);
                userManager.AddToRole(adminUser.Id, "Admin");
            }
            else if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }

            // =========================
            // TEACHER USER
            // =========================
            var teacherUser = userManager.FindByEmail(teacherEmail);

            if (teacherUser == null)
            {
                teacherUser = new ApplicationUser
                {
                    UserName = teacherEmail,
                    Email = teacherEmail
                };

                userManager.Create(teacherUser, password);
                userManager.AddToRole(teacherUser.Id, "Opetaja");
            }
            else if (!userManager.IsInRole(teacherUser.Id, "Opetaja"))
            {
                userManager.AddToRole(teacherUser.Id, "Opetaja");
            }

            // =========================
            // TEACHER PROFILE
            // =========================
            if (!context.Teachers.Any(t => t.ApplicationUserId == teacherUser.Id))
            {
                context.Teachers.Add(new Teacher
                {
                    Nimi = "Mari Maasikas",
                    Kvalifikatsioon = "Saksa keele õpetaja",
                    FotoPath = "mari.jpg",
                    ApplicationUserId = teacherUser.Id
                });

                context.SaveChanges();
            }
        }
    }
}
