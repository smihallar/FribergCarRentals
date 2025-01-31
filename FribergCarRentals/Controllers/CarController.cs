using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarRepository carRepository;

        public CarController(ICarRepository carRepository)
        {
            this.carRepository = carRepository;
        }

        // List all available cars
        public IActionResult Index()
        {
            var cars = carRepository.GetAll().Where(c => c.IsAvailable);
            return View(cars);
        }

        // View car details
        public IActionResult Details(int id)
        {
            var car = carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CarViewModel model)
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

        public IActionResult
    }
}
