using StockControlProject.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Entities.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Photo { get; set; }
        public string? Adress { get; set; }
        public string Password { get; set; }
        public UserRole UserRole { get; set; }

        public List<Order> Orders { get; set; }
        public User()
        {
            Orders = new List<Order>();             
        }
    }
}
