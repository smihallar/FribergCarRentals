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
            ViewData["ControllerName"] = "LoginRegister";
            return View(new LoginRegisterViewModel());
        }
        
        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email, Password")] LoginViewModel model)
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
                    ViewBag.LoginErrorMessage = "Fel inloggningsuppgifter";
                }
            }

            var loginRegisterViewModel = new LoginRegisterViewModel
            {
                Login = model,
                Register = new RegisterViewModel() // or populate with existing data if needed
            };

            ViewData["ControllerName"] = "LoginRegister";
            return View("Index", loginRegisterViewModel);
        }


      
        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("FirstName, LastName, Email, Password")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = customerRepository.GetByEmail(model.Email);
                if (existingCustomer != null)
                {
                    ViewBag.RegisterErrorMessage = "Denna email är redan registrerad!";
                    var loginRegisterViewModel = new LoginRegisterViewModel
                    {
                        Login = new LoginViewModel(),
                        Register = model
                    };
                    ViewData["ControllerName"] = "LoginRegister";
                    return View("Index", loginRegisterViewModel);
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

            var viewModel = new LoginRegisterViewModel
            {
                Login = new LoginViewModel(),
                Register = model
            };
            ViewData["ControllerName"] = "LoginRegister";
            return View("Index", viewModel);
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
