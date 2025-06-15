using System.Collections.Generic;

namespace Project.Models.Interfaces
{
    public interface IBookingRepository
    {
        void AddBooking(Booking booking);
        Booking GetBookingById(int id);
        List<Booking> GetBookingsByUser(string userId);
        void CancelBooking(int bookingId);
        List<Booking> GetAllBookings();
    }
}
