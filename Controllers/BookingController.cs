using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.Interfaces;
using System;
using System.Security.Claims;

namespace Project.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IBookingRepository _bookingRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(IMovieRepository movieRepo, IBookingRepository bookingRepo, UserManager<ApplicationUser> userManager)
        {
            _movieRepo = movieRepo;
            _bookingRepo = bookingRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var movies = _movieRepo.GetAll();
            return View(movies);
        }

        public IActionResult ToBook(int id)
        {
            var movie = _movieRepo.GetById(id);
            if (movie == null) return NotFound();

            ViewBag.Movie = movie;
            return View();
        }

        [HttpPost]
        public IActionResult ToBook(int movieId, DateTime showDate, int ticketsCount)
        {
            var movie = _movieRepo.GetById(movieId);
            if (movie == null || ticketsCount > movie.TicketsAvailable || ticketsCount <= 0 || showDate < DateTime.Today)
            {
                TempData["Error"] = "Invalid ticket count or show date.";
                return RedirectToAction("ToBook", new { id = movieId });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var booking = new Booking
            {
                MovieId = movieId,
                UserId = userId,
                ShowDate = showDate,
                TicketsCount = ticketsCount,
                BookingDate = DateTime.Now,
                Status = "Confirmed",
                TotalPrice = movie.Price * ticketsCount
            };

            _bookingRepo.AddBooking(booking);

            return RedirectToAction("Confirmation", new { id = booking.BookingId });
        }

        public IActionResult Confirmation(int id)
        {
            var booking = _bookingRepo.GetBookingById(id);
            if (booking == null || booking.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Unauthorized();
            }

            return View(booking);
        }

        public IActionResult List()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var bookings = _bookingRepo.GetBookingsByUser(userId);
            return View(bookings);
        }

        public IActionResult Cancel(int id)
        {
            var booking = _bookingRepo.GetBookingById(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (booking == null || booking.UserId != userId || booking.Status != "Confirmed")
                return Unauthorized();

            _bookingRepo.CancelBooking(id);
            TempData["Success"] = "Booking cancelled successfully.";
            return RedirectToAction("List");
        }
    }
}
