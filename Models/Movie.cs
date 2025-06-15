using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Movie
    {
        public int MovieId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

        public string PosterUrl { get; set; }

        public int TicketsAvailable { get; set; }

        // Navigation property
        public virtual ICollection<Booking> Bookings { get; set; }
    }

}
