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
        public virtual DbSet<FoodOrder> FoodOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure FoodOrder as a join entity
            modelBuilder.Entity<FoodOrder>()
                .HasKey(fo => new { fo.FoodID, fo.OrderID });

            modelBuilder.Entity<FoodOrder>()
                .HasOne(fo => fo.Food)
                .WithMany(f => f.FoodOrders)
                .HasForeignKey(fo => fo.FoodID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FoodOrder>()
                .HasOne(fo => fo.Order)
                .WithMany(o => o.FoodOrders)
                .HasForeignKey(fo => fo.OrderID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
