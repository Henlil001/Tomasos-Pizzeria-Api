using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [AllowNull]
        public int Points { get; set; }
        [AllowNull]
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
