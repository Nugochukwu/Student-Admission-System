using Microsoft.AspNetCore.Mvc;

namespace Student_Admission_System.Controllers
{
	public class StudentsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
