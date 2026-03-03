using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using KeeleSystem.Models;
using Microsoft.AspNet.Identity.Owin;

namespace KeeleSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminEnrollmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // /AdminEnrollments/Index
        public ActionResult Index()
        {
            var list = db.Registrations
                .Include(e => e.User)
                .Include(e => e.Training.Course)
                .Include(e => e.Training.Teacher)
                .OrderByDescending(e => e.Id)
                .ToList();

            return View(list);
        }

        // /AdminEnrollments/Confirm/5
        public ActionResult Confirm(int id)
        {
            var e = db.Registrations.Find(id);
            if (e == null) return HttpNotFound();

            e.Status = "Kinnitatud";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // /AdminEnrollments/Cancel/5
        public ActionResult Cancel(int id)
        {
            var e = db.Registrations.Find(id);
            if (e == null) return HttpNotFound();

            e.Status = "Tühistatud";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // /AdminEnrollments/Pending
        public ActionResult Pending()
        {
            var list = db.Registrations
                .Include(e => e.User)
                .Include(e => e.Training.Course)
                .Include(e => e.Training.Teacher)
                .Where(e => e.Status == "Ootel")
                .OrderByDescending(e => e.Id)
                .ToList();

            return View(list);
        }

        // /AdminEnrollments/EmailTraining/5
        public ActionResult EmailTraining(int id)
        {
            var training = db.Trainings
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .Include(t => t.Enrollments.Select(e => e.User))
                .FirstOrDefault(t => t.Id == id);

            if (training == null)
            {
                return HttpNotFound();
            }

            var recipients = GetRecipientIds(training.Enrollments);

            var vm = new TrainingEmailViewModel
            {
                TrainingId = training.Id,
                CourseName = training.Course?.Nimetus,
                TeacherName = training.Teacher?.Nimi,
                AlgusKuupaev = training.AlgusKuupaev,
                RecipientCount = recipients.Count,
                Subject = $"Teade koolitusele {training.Course?.Nimetus}"
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailTraining(TrainingEmailViewModel model)
        {
            var training = db.Trainings
                .Include(t => t.Course)
                .Include(t => t.Teacher)
                .Include(t => t.Enrollments.Select(e => e.User))
                .FirstOrDefault(t => t.Id == model.TrainingId);

            if (training == null)
            {
                return HttpNotFound();
            }

            var recipients = GetRecipientIds(training.Enrollments);
            if (recipients.Count == 0)
            {
                ModelState.AddModelError("", "Sellel koolitusel ei ole registreerunud õpilasi.");
            }

            if (!ModelState.IsValid)
            {
                model.CourseName = training.Course?.Nimetus;
                model.TeacherName = training.Teacher?.Nimi;
                model.AlgusKuupaev = training.AlgusKuupaev;
                model.RecipientCount = recipients.Count;
                return View(model);
            }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            foreach (var userId in recipients)
            {
                await userManager.SendEmailAsync(userId, model.Subject, model.Body);
            }

            TempData["Message"] = $"E-kiri saadetud {recipients.Count} õpilasele.";
            return RedirectToAction("Index");
        }

        private static List<string> GetRecipientIds(IEnumerable<Enrollment> enrollments)
        {
            return enrollments
                .Where(e => e.User != null && !string.IsNullOrWhiteSpace(e.User.Email))
                .Select(e => e.User.Id)
                .Distinct()
                .ToList();
        }
    }
}
