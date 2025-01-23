using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;

namespace FribergCarRentals.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public LoginRegisterController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        // GET: Index 
        public IActionResult Index()
        {
            var model = new LoginRegisterViewModel();
            return View(model); 
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = customerRepository.GetByEmail(model.Login.Email);
                if (customer != null && customer.Password == model.Login.Password)
                {
                    HttpContext.Session.SetString("CustomerEmail", customer.Email);
                    HttpContext.Session.SetInt32("CustomerId", customer.Id);

                    return RedirectToAction("Index", "Booking");

                }
                else
                {
                    ModelState.AddModelError("", "Det fungerade tyvärr inte att logga in, försök igen!");
                }
            }
            return View("Index", model);
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = customerRepository.GetByEmail(model.Register.Email);
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("Email", "Denna email är redan registrerad!");
                    return View("Index", model);
                }

                var newCustomer = new Customer
                {
                    FirstName = model.Register.FirstName,
                    LastName = model.Register.LastName,
                    Email = model.Register.Email,
                    Password = model.Register.Password
                };
                customerRepository.Add(newCustomer);
                
                HttpContext.Session.SetString("CustomerEmail", newCustomer.Email);
                HttpContext.Session.SetInt32("CustomerId", newCustomer.Id);

                return RedirectToAction("Index", "Home"); // ändra till bokningssidan.
            }
            return View("Index", "Home");
        }// GET: LoginController
       

        // GET: LoginController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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

        // GET: LoginController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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

        // GET: LoginController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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
    }
}
