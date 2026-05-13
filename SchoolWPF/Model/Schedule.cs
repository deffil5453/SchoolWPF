using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Text;

namespace SchoolWPF.Model
{
    public class Schedule
    {
        [Key] public int Id { get; set; }
        public int ClassId { get; set; }
        public Classe Class { get; set; } = null!;
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
        public int DayOfWeek { get; set; }
        public int LessonNumber { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int AcademicYearId { get; set; }
        // В Model/Schedule.cs добавьте свойство (не маппится в БД):
        [NotMapped]
        public string DayOfWeekDisplay
        {
            get
            {
                string[] days = { "", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };
                if (DayOfWeek >= 1 && DayOfWeek <= 6)
                {
                    return days[DayOfWeek];
                }
                return "";
            }
        }
    }
}
