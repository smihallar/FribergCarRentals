using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergCarRentals.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ICarRepository carRepository;

        public BookingController(IBookingRepository bookingRepository, ICarRepository carRepository)
        {
            this.bookingRepository = bookingRepository;
            this.carRepository = carRepository;
        }
        // GET: BookingController
        public IActionResult Index()
        {
            return View(new BookingViewModel());
        }

        [HttpPost]
        public IActionResult Index(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.EndDate <= model.StartDate)
                {
                    ModelState.AddModelError("EndDate", "Slutdatum måste vara efter startdatum");
                }
                else
                {
                    var availableCars = carRepository.GetAll()
                        .Where(car => !bookingRepository.GetAll()
                        .Any(booking => booking.CarId == car.Id &&
                        ((model.StartDate >= booking.StartDate && model.StartDate <= booking.EndDate) ||
                        (model.EndDate >= booking.StartDate && model.EndDate <= booking.EndDate))))
                        .ToList();

                    model.AvailableCars = availableCars;
                }
            }
            return View(model);
        }

        // GET: BookingController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: BookingController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.EndDate <= model.StartDate)
                {
                    ModelState.AddModelError("EndDate", "Slutdatum måste vara efter startdatum");
                }
                else
                {
                    var customerId = HttpContext.Session.GetInt32("CustomerId");
                    if (customerId.HasValue)
                    {
                        var car = carRepository.GetById(model.SelectedCarId);
                        if (car == null)
                        {
                            return NotFound();
                        }
                        var days = (model.EndDate - model.StartDate).Days;
                        model.TotalCost = car.PricePerDay * days;
                        var booking = new Booking
                        {
                            CarId = model.SelectedCarId,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            CustomerId = customerId.Value,
                            TotalCost = model.TotalCost
                        };
                        bookingRepository.Add(booking);
                        return RedirectToAction("Confirmation", "Booking", new { id = booking.Id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "LoginRegister");
                    }
                }
            }
            return View("Index", model);
        }
        public IActionResult Confirmation(int id)
        {
            var booking = bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            var car = carRepository.GetById(booking.CarId);
            if (car == null)
            {
                return NotFound();
            }
            ViewBag.CarName = car.Name;
            return View(booking);
        }

        // GET: BookingController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
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

        // GET: BookingController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Booking booking)
        {

            if (booking != null && booking.StartDate > DateTime.Now)
            {
                bookingRepository.Delete(booking);
                return RedirectToAction("List");
            }

            return View();
        }

        public IActionResult List()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            if (!customerId.HasValue)
            {
                return RedirectToAction("Index", "LoginRegister");
            }
            var bookings = bookingRepository.GetAll().Where(b => b.CustomerId == customerId.Value).ToList();
            return View(bookings);
        }
    }
}
