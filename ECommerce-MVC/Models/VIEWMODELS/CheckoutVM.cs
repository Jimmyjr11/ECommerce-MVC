namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class CheckoutVM
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public List<CartItemVM> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
