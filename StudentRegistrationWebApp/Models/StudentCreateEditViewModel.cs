using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationWebApp.Models
{
    public class StudentCreateEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int CourseId { get; set; }

        //CourseName it will optional by specifying string?
        public string? CourseName { get; set; }
    }
}