using ECommerce_MVC.Models.Model;
using System.Collections.Generic;

namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class OrderDetailsVM
    {
        public Order OrderHeader { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}