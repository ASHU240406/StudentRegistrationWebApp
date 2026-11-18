namespace StudentRegistrationWebApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        // Links this registration to the logged-in Identity account
        public string? UserId { get; set; }
    }
}