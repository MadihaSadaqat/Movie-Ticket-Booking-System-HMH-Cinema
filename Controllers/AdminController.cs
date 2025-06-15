using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Models.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;
using System.Linq;

namespace Project.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly IMovieRepository _movieRepo;
        private readonly IHubContext<MovieHub> _hubContext;
        private readonly IBookingRepository _bookingRepo;

        public AdminController(IMovieRepository movieRepo, IHubContext<MovieHub> hubContext, IBookingRepository bookingRepo)
        {
            _movieRepo = movieRepo;
            _hubContext = hubContext;
            _bookingRepo = bookingRepo;
        }



        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddMovie()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMovie(string title, string genre, decimal price, string posterUrl, int ticketsAvailable)
        {
            Movie movie = new Movie();
            movie.Title = title;
            movie.Genre = genre;
            movie.Price = price;
            movie.PosterUrl = posterUrl;
            movie.TicketsAvailable = ticketsAvailable;

            _movieRepo.Add(movie);
            TempData["Success"] = "Movie added successfully!";

            _hubContext.Clients.All.SendAsync("ReceiveMovieUpdate");

            return RedirectToAction("Index");
            
        }
      
        [HttpGet]
        public IActionResult EditMovie(int id)
        {
            var movie = _movieRepo.GetById(id);
            if (movie == null)
            {
                return NotFound(); 
            }
            return View(movie);
        }

        [HttpPost]
        public IActionResult EditMovie(int movieId,string title, string genre, decimal price, string posterUrl, int ticketsAvailable)
        {
            var updatedMovie = new Movie
            {
                MovieId = movieId,
                Title = title,
                Genre = genre,
                Price = price,
                PosterUrl = posterUrl,
                TicketsAvailable = ticketsAvailable
            };

            _movieRepo.Update(updatedMovie);
            TempData["Success"] = "Movie updated successfully!";

            _hubContext.Clients.All.SendAsync("ReceiveMovieUpdate");

            return RedirectToAction("AllMovies");
        }

        [HttpGet]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _movieRepo.GetById(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie); 
        }

        [HttpPost]
        [ActionName("DeleteMovie")] 
        public IActionResult DeleteMovieConfirmed(int movieId)
        {
            var movie = _movieRepo.GetById(movieId);
            if (movie == null)
            {
                return NotFound();
            }

            _movieRepo.Delete(movieId);
            TempData["Success"] = "Movie deleted successfully!";

            _hubContext.Clients.All.SendAsync("ReceiveMovieUpdate");

            return RedirectToAction("AllMovies");
        }

        public IActionResult AllMovies()
        {
            var movies = _movieRepo.GetAll(); 
            return View(movies);              
        }


        public IActionResult AllBookings()
        {
            var bookings = _bookingRepo.GetAllBookings()
                .Select(booking => new BookingViewModel
                {
                    PersonName = $"{booking.User.FirstName} {booking.User.LastName}",
                    MovieTitle = booking.Movie.Title,
                    ShowDate = booking.ShowDate,
                    TicketsCount = booking.TicketsCount,
                    ReferenceNumber = $"HMH-{booking.BookingId.ToString().PadLeft(6, '0')}",
                    Status = booking.Status
                })
                .ToList();

            return View(bookings);
        }
    }
}
