using System.ComponentModel.DataAnnotations;

namespace Tomasos_Pizzeria.Domain.Entities
{
    public class Ingredient
    {
        [Key]
        public int IngredientID { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        public virtual ICollection<Food>? Foods { get; set; }
    }
}
