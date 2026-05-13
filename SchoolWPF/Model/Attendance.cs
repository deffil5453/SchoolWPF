using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace SchoolWPF.Model
{
    public class Attendance
    {
        [Key] public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int ClassId { get; set; }
        public Classe Class { get; set; } = null!;

        public DateTime Date { get; set; }
        public int LessonNumber { get; set; }
        
        [Required, MaxLength(10)]
        public string Status { get; set; }

        [MaxLength(500)] public string? Comment { get; set; }
        public DateTime RecordedAt { get; set; }
        [MaxLength(100)] public string? RecordedBy { get; set; }
    }
}
