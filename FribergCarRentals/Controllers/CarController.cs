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
        private readonly IBookingRepository bookingRepository;

        public CarController(ICarRepository carRepository, IBookingRepository bookingRepository)
        {
            this.carRepository = carRepository;
            this.bookingRepository = bookingRepository;
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
            var model = new CarViewModel
            {
                ImageLinks = new List<string> { string.Empty } // Initialize with one empty string
            };
            return View(model);
        }

        // POST: 
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
                return RedirectToAction("List", "Car");
            }
            return View(model);
        }

        //GET/Car/Delete/5
        public IActionResult Delete(int id)
        {
            var car = carRepository.GetById(id);
            if (car == null || HttpContext.Session.GetInt32("AdminId") == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Car car)
        {
            var existingCar = carRepository.GetById(car.Id);
            if (!bookingRepository.GetAll().Any(b => b.CarId == existingCar.Id))
            {
                carRepository.Delete(existingCar.Id);
                return RedirectToAction("List", "Car");
            }
            ViewBag.DeleteCarMessage = "Bilen kan inte tas bort";
            return View(existingCar);
        }

        public IActionResult List()
        {
            var cars = carRepository.GetAll();
            return View(cars);
        }

        public IActionResult Edit(int id)
        {
            return View(carRepository.GetById(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Car car)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    carRepository.Update(car);
                }
                return RedirectToAction("List", "Car");
            }
            catch
            {
                ViewBag.EditCarError = "Bilen kunde inte ändras.";
                return View();
            }
        }

    }
}

