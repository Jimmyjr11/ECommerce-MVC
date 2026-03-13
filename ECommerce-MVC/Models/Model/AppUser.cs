using Microsoft.AspNetCore.Identity;

namespace ECommerce_MVC.Models.Model
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        // Navigation Properties (The "One" side)
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}