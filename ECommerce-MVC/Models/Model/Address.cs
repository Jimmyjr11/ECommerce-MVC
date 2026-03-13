using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.Model
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Zip { get; set; }
        public bool IsDefault { get; set; }

        // Foreign Key to User
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}