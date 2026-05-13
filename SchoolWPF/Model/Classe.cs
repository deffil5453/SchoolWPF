using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWPF.Model
{
    public class Classe
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(10)] public string ClassName { get; set; }
        
        public int? ClassTeacherId { get; set; }
        public Teacher? ClassTeacher { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Schedule> ScheduleEntries { get; set; } 
        public ICollection<Attendance> Attendances { get; set; } 
    }
}
