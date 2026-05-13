using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace SchoolWPF.Model
{
    public class Teacher
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100)] public string LastName { get; set; }
        [Required, MaxLength(100)] public string FirstName { get; set; }
        [MaxLength(100)] public string MiddleName { get; set; }
        [MaxLength(20)] public string? Phone { get; set; }
        [MaxLength(100)] public string? Email { get; set; }
        public ICollection<User> Users { get; set; }
        public bool IsClassTeacher { get; set; }
        public DateTime HireDate { get; set; }
        public ICollection<Classe> ManagedClasses { get; set; }
        public ICollection<Schedule> ScheduleEntries { get; set; }
    }
}