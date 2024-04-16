using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockControlProject.Entities.Entities
{
    public class Supplier : BaseEntity
    {
        public string SupplierName { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public List<Product> Products { get; set; }
        public Supplier()
        {
            Products = new List<Product>();
        }
    }
}
