namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class Cart
    {
        public List<CartItemVM> CartItems { get; set; }=new List<CartItemVM>();
        public decimal GrandTotal => CartItems.Sum(x => x.Total);
    }
}
