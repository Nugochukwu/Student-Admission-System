using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student_Admission_System.Models;

namespace Student_Admission_System.Controllers
{
	[Authorize(Roles = "Admin")] //Allows only the processing of requests by users authorised as admins in the system
	public class AuditLogsController : Controller
	{
		private readonly ApplicationDbContext _context;//Variable declaration
		public AuditLogsController(ApplicationDbContext context)
		{
			_context = context; //variable initialization
		}
	
	}
}
