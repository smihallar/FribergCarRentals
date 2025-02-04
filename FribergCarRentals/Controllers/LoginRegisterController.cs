using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System.Diagnostics;

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
            if (HttpContext.Session.GetInt32("CustomerId") != null)
            {
                ViewBag.LoginMessage = "Du är redan inloggad!";
                return RedirectToAction("Index", "Home");
            }
            ViewData["ControllerName"] = "LoginRegister";
            var model = new LoginRegisterViewModel();
            return View(model);
        }
        
        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email, Password")]LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var customer = customerRepository.GetByEmail(model.Email);
                if (customer != null && customer.Password == model.Password)
                {
                    HttpContext.Session.SetString("CustomerEmail", customer.Email);
                    HttpContext.Session.SetInt32("CustomerId", customer.Id);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Login", "Invalid login credentials.");
                    return View("Index", model);
                }
            }

            return View("Index", model);
        }


      
        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult Register( [Bind("FirstName, LastName, Email, Password")]RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var existingCustomer = customerRepository.GetByEmail(model.Email);
                if (existingCustomer != null)
                {
                    ViewBag.RegisterError = "Denna email är redan registrerad!";
                    return View("Index", model);
                }

                var newCustomer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password
                };
                customerRepository.Add(newCustomer);

                HttpContext.Session.SetString("CustomerEmail", newCustomer.Email);
                HttpContext.Session.SetInt32("CustomerId", newCustomer.Id);

                return RedirectToAction("Index", "Home");
            }
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
