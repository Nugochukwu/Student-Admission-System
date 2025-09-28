using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Admission_System.Models
{
	public class Student
	{
		public int StudentID { get; set; }
		public string ApplicationNumber { get; set; } = "";
		public string FirstName { get; set; } = "";
		public string LastName { get; set; } = "";
		public DateTime DateOfBirth { get; set; }
		public string Email { get; set; } = "";
		public string? PhoneNumber { get; set; }
		public string? Address { get; set; }
		public string? HighSchoolName { get; set; }
		[Column(TypeName = "decimal(5,2)")]
		public decimal? HighSchoolScore { get; set; }
		public DateTime RegistrationDate { get; set; }
		public DateTime LastUpdated { get; set; }

		//Foreign Key
		public int AdmissionStatusID { get; set; }
		public AdmissionStatus AdmissionStatus { get; set; } = null!;
	}
	public class AdmissionStatus
	{ 
		public int AdmissionStatusID { get; set; }
		public string Name { get; set; } = "";
		public ICollection<Student> Students { get; set; } = new List<Student>();
	}

	public class AuditLog
	{
		[Key]
		public int LogID { get; set; }

		// Student affected
		public int StudentID { get; set; }
		public Student Student { get; set; } = null!;

		// Admin who did it (from Identity)
		public string AdminID { get; set; } = "";
		public IdentityUser Admin { get; set; } = null!;

		// Action details
		public string Action { get; set; } = "";
		public string? OldStatus { get; set; }
		public string? NewStatus { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.Now;
	
	}


}
