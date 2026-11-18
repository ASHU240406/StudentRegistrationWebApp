using Microsoft.EntityFrameworkCore;

namespace StudentRegistrationWebApp.Models
{
    public class StudentCourseDbContext : DbContext
    {
        //Constructor Injection
        public StudentCourseDbContext(DbContextOptions<StudentCourseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }

        //Seed Data - create the table with some initial records
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //seeding data for the Course entity using FluentApi
            modelBuilder.Entity<Course>().HasData(
                new Course { CourseId = 1, CourseName = "ASP.NET Core MVC", CourseDuration = 3 },
                new Course { CourseId = 2, CourseName = "Angular", CourseDuration = 2 }
            );

            //modelBuilder.Entity<Student>().HasData();
        }
    }
}