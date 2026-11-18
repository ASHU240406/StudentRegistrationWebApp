using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentRegistrationWebApp.Data;
using System.Diagnostics;

namespace StudentRegistrationWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public HomeController(UserManager<ApplicationUser> userManager,
                               SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var user = await userManager.GetUserAsync(User);

                if (!await userManager.IsInRoleAsync(user!, "Admin") &&
                    !await userManager.IsInRoleAsync(user!, "Student"))
                {
                    await userManager.AddToRoleAsync(user!, "Student");
                    await signInManager.RefreshSignInAsync(user!);
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}