using Microsoft.EntityFrameworkCore;
using SchoolWPF.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace SchoolWPF.AppContext
{
    public class WpfDbContext : DbContext
    {
        public WpfDbContext()
        {
            Database.EnsureCreated();
        }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Schedule> ScheduleEntries { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        private const string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ScholLagutDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Администратор" },
                new Role { Id = 2, Name = "Teacher", Description = "Учитель" }
            );            

            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "Математика" },
                new Subject { Id = 2, Name = "Русский язык" },
                new Subject { Id = 3, Name = "Литература" },
                new Subject { Id = 4, Name = "Физическая культура" },
                new Subject { Id = 5, Name = "ИЗО" },
                new Subject { Id = 6, Name = "Музыка" },
                new Subject { Id = 7, Name = "История" },
                new Subject { Id = 8, Name = "Алгебра" },
                new Subject { Id = 9, Name = "Геометрия" },
                new Subject { Id = 10, Name = "Информатика" },
                new Subject { Id = 11, Name = "Физика" },
                new Subject { Id = 12, Name = "Биология" },
                new Subject { Id = 13, Name = "Химия" },
                new Subject { Id = 14, Name = "ОБЖ" },
                new Subject { Id = 15, Name = "Технология" },
                new Subject { Id = 16, Name = "География" },
                new Subject { Id = 17, Name = "Обществознание" },
                new Subject { Id = 18, Name = "История" }
            );

            // === Тестовые данные ===
            modelBuilder.Entity<Teacher>().HasData(
                new Teacher
                {
                    Id = 1,
                    LastName = "Смирнова",
                    FirstName = "Елена",
                    MiddleName = "Викторовна",
                    IsClassTeacher = true
                    // ❌ Убрали HireDate = DateTime.Now.AddYears(-3) — нельзя в HasData!
                }
            );

            modelBuilder.Entity<Classe>().HasData(
                new Classe { Id = 1, ClassName = "9А", ClassTeacherId = 1 }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    LastName = "Петров",
                    FirstName = "Алексей",
                    MiddleName ="Игоревич",
                    BirthDate = new DateTime(2009, 4, 12), // ✅ Статичная дата
                    ClassId = 1
                    // ❌ Убрали EnrollmentDate = DateTime.Now — нельзя в HasData!
                },
                new Student
                {
                    Id = 2,
                    LastName = "Иванова",
                    FirstName = "Мария",
                    MiddleName = "Александровна",
                    BirthDate = new DateTime(2009, 7, 23), // ✅ Статичная дата
                    ClassId = 1
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Login = "admin",
                    Password = "123",
                    RoleId = 1,
                    IsActive = true,
                    CreatedAt = new DateTime(2024, 1, 1, 12, 0, 0), // ✅ Статичная дата
                    
                }
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            }
        }
    }
}