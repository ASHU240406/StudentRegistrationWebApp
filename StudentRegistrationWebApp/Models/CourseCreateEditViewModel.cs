using System.ComponentModel.DataAnnotations;

namespace StudentRegistrationWebApp.Models
{
    public class CourseCreateEditViewModel
    {
        public int CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        [Range(1, 60)]
        public int CourseDuration { get; set; }
    }
}