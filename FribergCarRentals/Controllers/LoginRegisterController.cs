using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly CustomerRepository customerRepository;

        public LoginRegisterController(CustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        // GET: Index 
        public ActionResult Index()
        {
            var model = new LoginRegisterViewModel();
            return View(model);  // Pass the ViewModel to the view
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = customerRepository.GetByEmail(model.Login.Email);
                if (customer != null && customer.Password == model.Login.Password)
                {
                    // Handle login success (e.g., store user in session)
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Det fungerade tyvärr inte att logga in, försök igen!";
                }
            }
            return View("Index", model); // Return the Index view with validation errors
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _customerRepository.GetCustomerByEmail(model.RegisterEmail);
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("RegisterEmail", "Email is already in use.");
                    return View("Index", model);
                }

                var newCustomer = new Customer
                {
                    Email = model.RegisterEmail,
                    Password = model.RegisterPassword // Hash passwords in real scenarios
                };
                _customerRepository.AddCustomer(newCustomer);

                // Optionally log in the user after registration
                return RedirectToAction("Index", "Home");
            }
            return View("Index", model); // Return the Index view with validation errors
        }// GET: LoginController
       

        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
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

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
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

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
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
    }
}
