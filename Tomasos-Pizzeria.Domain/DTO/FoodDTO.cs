namespace Tomasos_Pizzeria.Domain.DTO
{
    public class FoodDTO
    {
        public int FoodID { get; set; }
        public string? Title { get; set; }       
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public List<int> IngredientsId { get; set; }
        public int CategoryId { get; set; }
    }
}
