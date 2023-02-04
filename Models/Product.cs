using System;
using System.Collections.Generic;

namespace SAMSUNG_4_YOU.Models
{
    public partial class Product
    {
        public Product()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int FkCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public double ProductPrice { get; set; }
        public int ProductQty { get; set; }
        public string ProductImage { get; set; } = null!;
        public string ProductDesc { get; set; } = null!;

        public virtual Category FkCategory { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
