using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Admission_System.Areas.Identity.Data;

namespace Student_Admission_System.Controllers
{
    [Authorize(Roles = "Admin")] //Allows only the processing of requests by users authorised as admins in the system
	public class AuditLogsController : Controller
	{
		private readonly ApplicationDbContext2 _context;//Variable declaration
		public AuditLogsController(ApplicationDbContext2 context)
		{
			_context = context; //variable initialization
		}

		public IActionResult Index()
		{
			var logs = _context.AuditLogs
				.OrderByDescending(l => l.Timestamp)
				.ToList();
			return View(logs);
		}

	}
}
