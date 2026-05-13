using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWPF.Model
{
    public class Subject
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100)] public string Name { get; set; }
        [MaxLength(10)] public string? Code { get; set; }
        public ICollection<Schedule> ScheduleEntries { get; set; }
    }
}
