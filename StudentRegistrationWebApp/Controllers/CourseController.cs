using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationWebApp.Models;

namespace StudentRegistrationWebApp.Controllers
{
    [Authorize] // must be logged in to view any Course pages
    public class CourseController : Controller
    {
        StudentCourseDbContext dbContext;

        public CourseController(StudentCourseDbContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            var courseList = dbContext.Courses.ToList();
            return View(courseList);
        }

        public IActionResult Details(int id)
        {
            var c = dbContext.Courses.Find(id);
            if (c == null)
            {
                return NotFound();
            }
            return View(c);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CourseCreateEditViewModel cv)
        {
            if (ModelState.IsValid)
            {
                Course c = new Course();
                c.CourseName = cv.CourseName;
                c.CourseDuration = cv.CourseDuration;

                dbContext.Courses.Add(c);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var c = dbContext.Courses.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            CourseCreateEditViewModel cv = new CourseCreateEditViewModel();
            cv.CourseId = c.CourseId;
            cv.CourseName = c.CourseName;
            cv.CourseDuration = c.CourseDuration;

            return View(cv);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, CourseCreateEditViewModel cvModified)
        {
            var c = dbContext.Courses.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                c.CourseName = cvModified.CourseName;
                c.CourseDuration = cvModified.CourseDuration;

                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cvModified);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var c = dbContext.Courses.Find(id);
            if (c == null)
            {
                return NotFound();
            }

            dbContext.Courses.Remove(c);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}