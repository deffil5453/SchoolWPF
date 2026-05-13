using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace SchoolWPF.Model
{
    public class User
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(50)] public string Login { get; set; } = "";
        [Required, MaxLength(50)] public string Password { get; set; } = "";
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        // ✅ Как в вашем примере: внешний ключ + навигация на Teacher
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
