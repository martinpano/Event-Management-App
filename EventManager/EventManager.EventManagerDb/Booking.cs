using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Db
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int NumberOfTickets { get; set; }
        public DateTime BookingDate { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Event Event { get; set; }
    }
}
