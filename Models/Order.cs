using System;
using System.Collections.Generic;

namespace SAMSUNG_4_YOU.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public int FkCustomerId { get; set; }
        public double TotalPrice { get; set; }
        public string OrderDate { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string AddressDetail { get; set; } = null!;
        public int OrderStatus { get; set; }

        public virtual Customer FkCustomer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
