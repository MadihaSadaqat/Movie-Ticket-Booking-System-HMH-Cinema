using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public DateTime ShowDate { get; set; }

        public int TicketsCount { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual ApplicationUser User { get; set; }
    }

}
