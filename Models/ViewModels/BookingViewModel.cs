using System;

namespace Project.Models
{
    public class BookingViewModel
    {
        public string PersonName { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ShowDate { get; set; }
        public int TicketsCount { get; set; }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
    }
}