using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
        private readonly ICarRepository carRepository;

        public AdminController(IAdminRepository adminRepository, ICarRepository carRepository)
        {
            this.adminRepository = adminRepository;
            this.carRepository = carRepository;
        }
        // GET: AdminController
        public IActionResult Index()
        
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        
        public IActionResult Login()
        {
            if (IsAdminLoggedIn())
            {
                return RedirectToAction("Index");
            }

            ViewData["ControllerName"] = "Admin";
            return View("LoginForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = adminRepository.GetByEmail(model.Email);
                if (admin != null && admin.Password == model.Password)
                {

                    HttpContext.Session.SetString("AdminEmail", admin.Email);
                    HttpContext.Session.SetInt32("AdminId", admin.Id);
                    return View("Index", "Admin");
                }
                else
                {
                    ViewBag.LoginErrorMessage = "Fel inloggningsuppgifter";


                    ViewData["ControllerName"] = "Admin";
                    return View("LoginForm", model);
                }
            }
            else
            {
                ViewData["ControllerName"] = "Admin";
                return View("LoginForm", model);
            }
        }
        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetInt32("AdminId") != null;
        }
    }
}
