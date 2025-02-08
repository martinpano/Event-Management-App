using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Db
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }  // e.g., "Admin", "User"

        // Navigation Properties
        public ICollection<Booking> Bookings { get; set; }
    }
}
