using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tomasos_Pizzeria.Domain.Entities
{
    public class FoodOrder
    {
        public int FoodID { get; set; }
        public Food Food { get; set; }

        public int OrderID { get; set; }
        public Order Order { get; set; }

        
        public int Quantity { get; set; }
    }
}
