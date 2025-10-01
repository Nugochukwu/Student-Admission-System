using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			
            Console.WriteLine("POST Register called"); // debug
            

			var pendingStatus = _context.AdmissionStatuses.FirstOrDefault(s => s.Name == "PENDING");
			if (pendingStatus != null)
			{
				
				student.AdmissionStatusID = pendingStatus.AdmissionStatusID;
			}
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation error: {error.ErrorMessage}");
            }
            // Generate unique Application Number (year + random)
            student.ApplicationNumber = $"{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
			student.RegistrationDate = DateTime.Now;
            student.LastUpdated = DateTime.Now;

            if (ModelState.IsValid)
			{
				
				_context.Students.Add(student);
				_context.SaveChanges();

				ViewBag.ApplicationNumber = student.ApplicationNumber;
                Console.WriteLine("POST Register Succeeded");
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
                .Include(s => s.AdmissionStatus)
                .Where(s => s.ApplicationNumber == applicationNumber)
				.FirstOrDefault();
			Console.WriteLine("Check Status is being Hit");
			if (student == null)
			{
				ViewBag.Message = "Invalid Application Number!";
				return View();
			}

            switch (student.AdmissionStatus.Name.ToUpper())
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

//validation for duplicate emails