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

            // Update IsCompleted status
            if (booking.EndDate <= DateTime.Now && !booking.IsCompleted)
            {
                booking.IsCompleted = true;
                bookingRepository.Update(booking);
            }

            var customer = customerRepository.GetById(booking.CustomerId);
            if (customer == null)
            {
                return NotFound();
            }

            var car = carRepository.GetById(booking.CarId);
            if (car == null)
            {
                return NotFound();
            }
            var viewModel = new CustomerBookingViewModel
            {
                Customer = customer,
                Booking = booking,
                Car = car
            };

            return View(viewModel);
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
                        HttpContext.Session.SetString("RedirectToBooking", "true");
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
            var customer = customerRepository.GetById(booking.CustomerId);

            CustomerBookingViewModel model = new CustomerBookingViewModel
            {
                Booking = booking,
                Car = car,
                Customer = customer
            };
            return View(model);
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
            var existingBooking = bookingRepository.GetById(booking.Id);
            if (existingBooking != null && existingBooking.IsCompleted == false && existingBooking.StartDate > DateTime.Now)
            {
                bookingRepository.Delete(existingBooking);
                return RedirectToAction("List", "Booking");
            }
            ViewBag.ErrorMessage = "Bokningen kan inte tas bort";
            return View(existingBooking);
        }

        public IActionResult List()
        {
            var customerId = HttpContext.Session.GetInt32("CustomerId");
            var adminId = HttpContext.Session.GetInt32("AdminId");

            var bookings = bookingRepository.GetAll().ToList();

            // Update IsCompleted status
            foreach (var booking in bookings)
            {
                if (booking.EndDate <= DateTime.Now && !booking.IsCompleted)
                {
                    booking.IsCompleted = true;
                    bookingRepository.Update(booking);
                }
            }

            if (adminId.HasValue)
            {
                return View(bookings);
            }
            else if (customerId.HasValue)
            {
                var customerBookings = bookings.Where(b => b.CustomerId == customerId.Value).ToList();
                return View(customerBookings);
            }
            else
            {
                return RedirectToAction("Index", "LoginRegister");
            }
        }
    }
}
