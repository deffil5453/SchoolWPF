using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace SchoolWPF.Model
{
    public class Student
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100)] public string LastName { get; set; }
        [Required, MaxLength(100)] public string FirstName { get; set; }
        [MaxLength(100)] public string? MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(1)] public string Gender { get; set; } = "М";
        public int ClassId { get; set; }
        public Classe Class { get; set; } = null!;
        public DateTime EnrollmentDate { get; set; }
        [MaxLength(20)] public string Status { get; set; } = "Active";
        [MaxLength(100)] public string? ParentContact { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
    }
}
