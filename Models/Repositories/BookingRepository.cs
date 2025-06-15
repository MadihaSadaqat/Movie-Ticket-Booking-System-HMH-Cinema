using Project.Data;
using Project.Models;
using Project.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Project.Models.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddBooking(Booking booking)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == booking.MovieId);
            if (movie != null && movie.TicketsAvailable >= booking.TicketsCount)
            {
                movie.TicketsAvailable -= booking.TicketsCount;
                booking.TotalPrice = movie.Price * booking.TicketsCount;
                booking.Status = "Confirmed";
                booking.BookingDate = DateTime.Now;

                _context.Bookings.Add(booking);
                _context.SaveChanges();
            }
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.Movie)
                .FirstOrDefault(b => b.BookingId == id);
        }

        public List<Booking> GetBookingsByUser(string userId)
        {
            return _context.Bookings
                .Include(b => b.Movie)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToList();
        }

        public void CancelBooking(int bookingId)
        {
            var booking = _context.Bookings.Include(b => b.Movie).FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null && booking.Status == "Confirmed")
            {
                booking.Status = "Cancelled";
                booking.Movie.TicketsAvailable += booking.TicketsCount;
                _context.SaveChanges();
            }
        }

        public List<Booking> GetAllBookings()
        {
            return _context.Bookings
                .Include(b => b.Movie)
                .Include(b => b.User)
                .OrderByDescending(b => b.BookingDate)
                .ToList();
        }
    }
}
