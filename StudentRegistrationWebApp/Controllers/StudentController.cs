using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentRegistrationWebApp.Data;
using StudentRegistrationWebApp.Models;

namespace StudentRegistrationWebApp.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        StudentCourseDbContext dbContext;
        UserManager<ApplicationUser> userManager;

        public StudentController(StudentCourseDbContext context, UserManager<ApplicationUser> userManager)
        {
            dbContext = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var sList = dbContext.Students.Include("Course").ToList();
            return View(sList);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var s = dbContext.Students.Include("Course").SingleOrDefault(st => st.StudentId == id);
            if (s == null) return NotFound();
            return View(s);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var s = dbContext.Students.Find(id);
            if (s == null) return NotFound();
            dbContext.Students.Remove(s);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var s = dbContext.Students.Include("Course").SingleOrDefault(st => st.StudentId == id);
            if (s == null) return NotFound();

            var sv = new StudentCreateEditViewModel
            {
                Id = s.StudentId,
                FullName = s.FullName,
                City = s.City,
                CourseId = s.CourseId
            };

            ViewBag.CourseList = GetCourseData();
            return View(sv);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(int id, StudentCreateEditViewModel svModified)
        {
            var s = dbContext.Students.Include("Course").SingleOrDefault(st => st.StudentId == id);
            if (s == null) return NotFound();

            if (ModelState.IsValid)
            {
                s.FullName = svModified.FullName;
                s.City = svModified.City;
                s.CourseId = svModified.CourseId;

                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseList = GetCourseData();
            return View(svModified);
        }

        [Authorize(Roles = "Student")]
        public IActionResult Create()
        {
            var userId = userManager.GetUserId(User);
            var alreadyRegistered = dbContext.Students.Any(s => s.UserId == userId);
            if (alreadyRegistered)
            {
                return RedirectToAction("MyProfile");
            }

            ViewBag.CourseList = GetCourseData();
            return View();
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public IActionResult Create(StudentCreateEditViewModel sv)
        {
            var userId = userManager.GetUserId(User);
            var alreadyRegistered = dbContext.Students.Any(s => s.UserId == userId);
            if (alreadyRegistered)
            {
                return RedirectToAction("MyProfile");
            }

            if (ModelState.IsValid)
            {
                var s = new Student
                {
                    FullName = sv.FullName,
                    City = sv.City,
                    CourseId = sv.CourseId,
                    UserId = userId
                };

                dbContext.Students.Add(s);
                dbContext.SaveChanges();

                TempData["Message"] = "You have successfully registered for the course!";
                return RedirectToAction("MyProfile");
            }
            ViewBag.CourseList = GetCourseData();
            return View(sv);
        }

        [Authorize(Roles = "Student")]
        public IActionResult MyProfile()
        {
            var userId = userManager.GetUserId(User);
            var s = dbContext.Students.Include("Course").SingleOrDefault(st => st.UserId == userId);

            if (s == null)
            {
                return RedirectToAction("Create");
            }
            return View(s);
        }

        [Authorize(Roles = "Student")]
        public IActionResult EditProfile()
        {
            var userId = userManager.GetUserId(User);
            var s = dbContext.Students.SingleOrDefault(st => st.UserId == userId);
            if (s == null) return RedirectToAction("Create");

            var sv = new StudentCreateEditViewModel
            {
                Id = s.StudentId,
                FullName = s.FullName,
                City = s.City,
                CourseId = s.CourseId
            };
            ViewBag.CourseList = GetCourseData();
            return View(sv);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        public IActionResult EditProfile(StudentCreateEditViewModel svModified)
        {
            var userId = userManager.GetUserId(User);
            var s = dbContext.Students.SingleOrDefault(st => st.UserId == userId);
            if (s == null) return RedirectToAction("Create");

            if (ModelState.IsValid)
            {
                s.FullName = svModified.FullName;
                s.City = svModified.City;
                dbContext.SaveChanges();

                TempData["Message"] = "Your profile has been updated.";
                return RedirectToAction("MyProfile");
            }
            ViewBag.CourseList = GetCourseData();
            return View(svModified);
        }

        SelectList GetCourseData()
        {
            var courseList = dbContext.Courses.ToList();
            return new SelectList(courseList, "CourseId", "CourseName");
        }
    }
}