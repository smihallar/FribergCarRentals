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
        public ActionResult Index()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        //GET: AdminController/Admin/
        public IActionResult Login()
        { 
                ViewData["ControllerName"] = "Admin";
            return View("LoginForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email, Password")] LoginViewModel model)
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
                    ModelState.AddModelError("Login", "Felaktiga inloggningsuppgifter!");

                    
                        ViewData["ControllerName"] = "Admin";
                        return View("Index", model);
                }
            }
            else
            {
                ViewData["ControllerName"] = "Admin";
                return View("LoginForm", model);
            }
        }
        // GET: AdminController/AddCar
        public IActionResult AddCar()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login");
            }
            return View(new CarViewModel());
        }

        // POST: AdminController/AddCar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCar(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    Name = model.Name,
                    PricePerDay = model.PricePerDay,
                    ImageLinks = model.ImageLinks,
                    IsAvailable = model.IsAvailable
                };

                carRepository.Add(car);
                return RedirectToAction("Index", "Admin");
            }
            return View(model);
        }
        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //public IActionResult ManageCars()
        //{
        //    if (!IsAdminLoggedIn())
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    return View();
        //}
        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetInt32("AdminId") != null;
        }
    }
}
