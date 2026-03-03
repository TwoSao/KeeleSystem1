using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using KeeleSystem.Models;
using Microsoft.AspNet.Identity;

namespace KeeleSystem.Controllers
{
    public class EnrollmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Enrollment
        public ActionResult Index()
        {
            var list = db.Registrations
                .Include(e => e.Training)
                .Include(e => e.User)
                .ToList();

            return View(list);
        }

        [Authorize(Roles = "Opilane")]
        public ActionResult MyCourses()
        {
            string userId = User.Identity.GetUserId();

            var list = db.Registrations
                .Include(e => e.Training.Course)
                .Where(e => e.ApplicationUserId == userId)
                .ToList();

            return View(list);
        }

        [Authorize(Roles = "Opilane")]
        public ActionResult Create(int trainingId)
        {
            var training = db.Trainings
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .Include(t => t.Enrollments)
                .FirstOrDefault(t => t.Id == trainingId);

            if (training == null)
            {
                return HttpNotFound();
            }

            string userId = User.Identity.GetUserId();
            if (IsAlreadyRegistered(trainingId, userId))
            {
                TempData["Message"] = "Sa oled juba sellele koolitusele registreerunud.";
                return RedirectToAction("MyCourses");
            }

            int osalejaid = training.Enrollments?.Count ?? 0;
            if (osalejaid >= training.MaxOsalejaid)
            {
                TempData["Message"] = "GRUPP TÄIS - registreerimine pole võimalik.";
                return RedirectToAction("Index", "Trainings");
            }

            return View(training);
        }

        [HttpPost]
        [Authorize(Roles = "Opilane")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(int trainingId)
        {
            var training = db.Trainings
                .Include(t => t.Enrollments)
                .FirstOrDefault(t => t.Id == trainingId);

            if (training == null)
            {
                return HttpNotFound();
            }

            string userId = User.Identity.GetUserId();
            if (IsAlreadyRegistered(trainingId, userId))
            {
                TempData["Message"] = "Sa oled juba sellele koolitusele registreerunud.";
                return RedirectToAction("MyCourses");
            }

            int osalejaid = training.Enrollments?.Count ?? 0;
            if (osalejaid >= training.MaxOsalejaid)
            {
                TempData["Message"] = "GRUPP TÄIS - registreerimine pole võimalik.";
                return RedirectToAction("Index", "Trainings");
            }

            var enrollment = new Enrollment
            {
                TrainingId = trainingId,
                ApplicationUserId = userId,
                Status = "Ootel"
            };

            db.Registrations.Add(enrollment);
            db.SaveChanges();

            TempData["Message"] = "Registreerimine edukas! Staatus: Ootel.";
            return RedirectToAction("MyCourses");
        }

        private bool IsAlreadyRegistered(int trainingId, string userId)
        {
            return db.Registrations.Any(e =>
                e.TrainingId == trainingId && e.ApplicationUserId == userId);
        }
    }
}
