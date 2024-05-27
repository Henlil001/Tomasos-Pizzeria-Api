using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Identity
{
    public class PizzeriaDbContext : IdentityDbContext<ApplicationUser>
    {
        public PizzeriaDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
    }
}
