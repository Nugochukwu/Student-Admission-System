using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Admission_System.Areas.Identity.Data;
using Student_Admission_System.Models;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext2 _context;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(ApplicationDbContext2 context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // View all applications
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult AuditLogs()
    {
        var logs = _context.AuditLogs
       .Include(l => l.Admin)   // eager load related Admin
       .Include(l => l.Student) // eager load related Student
       .OrderByDescending(l => l.Timestamp)
       .ToList();
        return View("Logs",logs);
    }
    public IActionResult Dashboard(string search, int? statusId)
    {
        var query = _context.Students
            .Include(s => s.AdmissionStatus)
            .AsQueryable();
        //Filter
        if(!string.IsNullOrEmpty(search))
        {
            query = query.Where(s =>
            s.FirstName.Contains(search) ||
            s.LastName.Contains(search) ||
            s.ApplicationNumber.Contains(search));
        }

        // Status filter
        if (statusId.HasValue)
        {
            query = query.Where(s => s.AdmissionStatusID == statusId.Value);
        }

        // Populate dropdown list
        ViewBag.Statuses = _context.AdmissionStatuses.ToList();

        var students = query.ToList();
        return View(students);
    }

   [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var student = _context.Students.FirstOrDefault(s => s.StudentID == id);
        if (student == null)
        {
            return NotFound();
        }

        // Map string status to AdmissionStatusID
        var admissionStatus = _context.AdmissionStatuses
            .FirstOrDefault(s => s.Name.ToUpper() == status.ToUpper());
        if (admissionStatus == null)
        {
            return BadRequest("Invalid Status");
        }
        // Update student status
        student.AdmissionStatusID = admissionStatus.AdmissionStatusID;
        student.LastUpdated = DateTime.Now;




        // Get logged-in admin
        var adminUser = await _userManager.GetUserAsync(User);

        var statusId = student.AdmissionStatusID;
        var log = new AuditLog
        {
            Timestamp = DateTime.Now,
            AdminID = adminUser.Id,
            StudentID = student.StudentID,
            Action = $"Changed status to {admissionStatus.Name}"
        };
        

        _context.AuditLogs.Add(log);
        _context.SaveChanges();

        return RedirectToAction("Dashboard");
    }
}
