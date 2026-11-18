using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentRegistrationWebApp.Models
{
    public class Course
    {
        [Key] //Data Annotation Attribute to specify Primary Key
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public int CourseDuration { get; set; }

        //one to many relation
        //Navigation Property to retreive List of Students
        public virtual List<Student> Students { get; set; }
    }
}