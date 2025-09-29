using Microsoft.AspNetCore.Mvc;
using Student_Admission_System.Areas.Identity.Data;
using Student_Admission_System.Models;

namespace Student_Admission_System.Controllers
{
    public class StudentsController : Controller
	{
		private readonly ApplicationDbContext2 _context;

		public StudentsController(ApplicationDbContext2 context)
		{
			_context = context;
		}



        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }



        // GET: Registration Form
        public IActionResult Register()
		{
			return View();
		}

		// POST: Register Student
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Register(Student student)
		{
			if (ModelState.IsValid)
			{
				// Generate unique Application Number (year + random)
				student.ApplicationNumber = $"{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
				student.RegistrationDate = DateTime.Now;
				student.LastUpdated = DateTime.Now;

				// Default status = PENDING
				var pendingStatus = _context.AdmissionStatuses.FirstOrDefault(s => s.Name == "PENDING");
				student.AdmissionStatusID = pendingStatus.AdmissionStatusID;

				_context.Students.Add(student);
				_context.SaveChanges();

				ViewBag.ApplicationNumber = student.ApplicationNumber;
				return View("RegistrationSuccess");
			}

			return View(student);
		}

		// GET: Check Status Page
		public IActionResult CheckStatus()
		{
			return View();
		}

		// POST: Handle Status Check
		[HttpPost]
		public IActionResult CheckStatus(string applicationNumber)
		{
			var student = _context.Students
				.Where(s => s.ApplicationNumber == applicationNumber)
				.Select(s => new { s.FirstName, s.LastName, Status = s.AdmissionStatus.Name })
				.FirstOrDefault();

			if (student == null)
			{
				ViewBag.Message = "Invalid Application Number!";
				return View();
			}

			switch (student.Status.ToUpper())
			{
				case "PENDING":
					ViewBag.Message = "Your application is under review. Please check back later.";
					break;
				case "ACCEPTED":
					ViewBag.Message = "Congratulations! Your application has been accepted.";
					break;
				case "REJECTED":
					ViewBag.Message = "We regret to inform you that your application was not successful.";
					break;
				default:
					ViewBag.Message = "Unknown status.";
					break;
			}

			return View();
		}
	}
}
