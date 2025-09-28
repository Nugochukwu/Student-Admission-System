using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Student_Admission_System.Models;

namespace Student_Admission_System.Controllers
{
	public class AdminController : Controller
	{
		private readonly ApplicationDbContext _context;
		public AdminController(ApplicationDbContext context)
		{
			_context = context;
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
