using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tomasos_Pizzeria.Domain.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        public DateTime Date { get; set; }
        [Required]
        [StringLength(50)]
        public string Status { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Tillåter exakt två decimaler
        public decimal TotalPrice { get; set; }
        public bool Discount { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ICollection<Food> Foods { get; set; }
        public ICollection<FoodOrder> FoodOrders { get; set; }
    }
}
