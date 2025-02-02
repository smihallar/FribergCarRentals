using FribergCarRentals.Data;
using FribergCarRentals.Models;
using FribergCarRentals.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FribergCarRentals.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly ICarRepository carRepository;
        private readonly ICustomerRepository customerRepository;

        public BookingController(IBookingRepository bookingRepository, ICarRepository carRepository, ICustomerRepository customerRepository)
        {
            this.bookingRepository = bookingRepository;
            this.carRepository = carRepository;
            this.customerRepository = customerRepository;
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
                var startDateTime = model.StartDate.Date + model.StartTime;
                var endDateTime = model.EndDate.Date + model.EndTime;

                if (endDateTime <= startDateTime)
                {
                    ModelState.AddModelError("EndDate", "Slutdatum måste vara efter startdatum");
                }
                else
                {
                    var availableCars = carRepository.GetAll()
                        .Where(car => !bookingRepository.GetAll()
                        .Any(booking => booking.CarId == car.Id &&
                        ((startDateTime >= booking.StartDate && startDateTime <= booking.EndDate) ||
                        (endDateTime >= booking.StartDate && endDateTime <= booking.EndDate))))
                        .ToList();

                    model.AvailableCars = availableCars;
                }
            }
            return View(model);
        }

        // GET: BookingController/Details/5
        public IActionResult Details(int id)
        {
            var booking = bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
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
                var startDateTime = model.StartDate.Date + model.StartTime;
                var endDateTime = model.EndDate.Date + model.EndTime;

                if (endDateTime <= startDateTime)
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
                        var days = (endDateTime - startDateTime).Days;
                        model.TotalCost = car.PricePerDay * days;
                        var booking = new Booking
                        {
                            CarId = model.SelectedCarId,
                            StartDate = startDateTime,
                            EndDate = endDateTime,
                            CustomerId = customerId.Value,
                            TotalCost = model.TotalCost
                        };
                        bookingRepository.Add(booking);
                        customerRepository.AddBooking((int)customerId, booking);
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
            var booking = bookingRepository.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Booking booking)
        {
            if (booking != null && booking.StartDate > DateTime.Now && booking.EndDate > DateTime.Now)
            {
                bookingRepository.Delete(booking);
                return RedirectToAction("List", "Booking");
            }
            ViewBag.ErrorMessage = "Bokningen kan inte tas bort";
            return View(booking);
        }

        public IActionResult List()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var adminId = HttpContext.Session.GetInt32("AdminId");

            if (adminId.HasValue)
            {
                var bookings = bookingRepository.GetAll().ToList();
                return View(bookings);
            }
            else if (customerId.HasValue)
            {
                var bookings = bookingRepository.GetAll().Where(b => b.CustomerId == customerId.Value).ToList();
                return View(bookings);
            }
            else
            {
                // No user is signed in, redirect to login
                return RedirectToAction("Index", "LoginRegister");
            }
        }
    }
}
