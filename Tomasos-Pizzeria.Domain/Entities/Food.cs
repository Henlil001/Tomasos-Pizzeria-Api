using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tomasos_Pizzeria.Domain.Entities
{
    public class Food
    {
        [Key]
        public int FoodID { get; set; }
        [Required]
        [StringLength(150)]
        public string? Title { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Tillåter exakt två decimaler
        public decimal Price { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Category Category { get; set; }
    }
}
