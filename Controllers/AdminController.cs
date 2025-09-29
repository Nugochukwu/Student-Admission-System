using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Student_Admission_System.Areas.Identity.Data;
using Student_Admission_System.Models;

namespace Student_Admission_System.Controllers
{
    [Authorize(Roles = "Admin")] // only admins can access
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext2 _context;
		private readonly UserManager<IdentityUser> _userManager;

		public AdminController(ApplicationDbContext2 context, UserManager<IdentityUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

        // GET: Dashboard
        public IActionResult Dashboard(string search, int? statusId)
        {
            var students = _context.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s => s.FirstName.Contains(search) ||
                                               s.LastName.Contains(search) ||
                                               s.ApplicationNumber.Contains(search));
            }

            if (statusId.HasValue)
            {
                students = students.Where(s => s.AdmissionStatusID == statusId.Value);
            }

            ViewBag.Statuses = _context.AdmissionStatuses.ToList();
            return View(students.ToList());
        }


        // GET: Edit Status
        public IActionResult EditStatus(int id)
		{
			var student = _context.Students.Find(id);
			if (student == null) return NotFound();

			ViewBag.Statuses = _context.AdmissionStatuses.ToList();
			return View(student);
		}

		// POST: Update Status
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult EditStatus(int id, int newStatusId)
		{
			var student = _context.Students.Find(id);
			if (student == null) return NotFound();

			var oldStatus = student.AdmissionStatusID;
			student.AdmissionStatusID = newStatusId;
			student.LastUpdated = DateTime.Now;

			_context.SaveChanges();

			// Log action
			var adminId = _userManager.GetUserId(User);
			var log = new AuditLog
			{
				StudentID = student.StudentID,
				AdminID = adminId,
				Action = "Changed Status",
				OldStatus = _context.AdmissionStatuses.Find(oldStatus)?.Name,
				NewStatus = _context.AdmissionStatuses.Find(newStatusId)?.Name,
				Timestamp = DateTime.Now
			};

			_context.AuditLogs.Add(log);
			_context.SaveChanges();

			return RedirectToAction("Dashboard");
		}
	}
}
