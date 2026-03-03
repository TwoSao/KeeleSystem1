using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using KeeleSystem.Models;
using System.Data.Entity;

namespace KeeleSystem.Controllers
{
    [Authorize(Roles = "Opetaja")]
    public class TeacherController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Teacher/Index  (Töölaud)
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();

            // Найти профиль учителя по ApplicationUserId
            var teacher = db.Teachers.FirstOrDefault(t => t.ApplicationUserId == userId);
            if (teacher == null)
            {
                return Content("Teacher profiil puudub (Teacher.ApplicationUserId seos puudu).");
            }

            // Найти trainings этого учителя и подтянуть нужные связи
            var trainings = db.Trainings
                .Include(t => t.Course)
                .Include(t => t.Enrollments.Select(e => e.User))
                .Where(t => t.TeacherId == teacher.Id)
                .ToList();

            var vm = new TeacherDashboardVM
            {
                TeacherName = teacher.Nimi,
                Trainings = trainings
            };

            return View(vm);
        }
    }

    // простой ViewModel (чтобы удобно рендерить)
    public class TeacherDashboardVM
    {
        public string TeacherName { get; set; }
        public System.Collections.Generic.List<Training> Trainings { get; set; }
    }
}
