﻿using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        // GET: CustomerController
        public IActionResult Index()
        {
            var customers = customerRepository.GetAll();
            return View(customers);
        }

        // GET: CustomerController/Details/5
        public IActionResult Details(int id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: CustomerController/Create
        public IActionResult Create()
        {
            return View(new CustomerViewModel());
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password
                };

                customerRepository.Add(customer);
                return RedirectToAction("Create", "Customer");
            }
            return View(model);
        }

        // GET: CustomerController/Edit/5
        public IActionResult Edit(int id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new CustomerViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Password = customer.Password
            };

            return View(model);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, CustomerViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var customer = customerRepository.GetById(id);
                    if (customer == null)
                    {
                        return NotFound();
                    }

                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.Email = model.Email;
                    customer.Password = model.Password;

                    customerRepository.Update(customer);
                    return RedirectToAction("Index", "Customer");
                }
                return View(model);
            }
            catch
            {
                ViewBag.EditCustomerError = "Kundens uppgifter kunde inte ändras.";
                return View(model);
            }
        }

        // GET: CustomerController/Delete/5
        public IActionResult Delete(int id)
        {
            var customer = customerRepository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Customer customer)
        {
            var existingCustomer = customerRepository.GetById(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.Bookings = customerRepository.GetBookingsByCustomerId(id).ToList();

            if (existingCustomer.Bookings == null || !existingCustomer.Bookings.Any())
            {
                customerRepository.Delete(existingCustomer);
                ViewBag.DeleteCustomerMessage = "Kunden har tagits bort.";
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                existingCustomer.FirstName = "Anonym";
                existingCustomer.LastName = "Anonym";
                existingCustomer.Email = "anonym@example.se";
                existingCustomer.Password = "anonym";
                customerRepository.Update(existingCustomer);
                ViewBag.DeleteCustomerMessage = "Kunden har anonymiserats.";
                return RedirectToAction("Index", "Customer");
            }
        }
    }
}
