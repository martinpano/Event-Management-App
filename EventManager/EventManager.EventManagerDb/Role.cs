using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Db
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }  // e.g., "Admin", "User"

        // Navigation Properties
        public ICollection<User> Users { get; set; }
    }
}
