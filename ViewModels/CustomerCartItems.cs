using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMSUNG_4_YOU.ViewModels
{
    public class CustomerCartItems
    {
        [Key]
        public int cartId { get; set; }
        public int customerId { get; set; }
        public int productId { get; set; }
        [DisplayName("Product Name")]
        public string productName { get; set; } = null!;
        [DisplayName("Product Price")]
        public double productPrice { get; set; }
        [DisplayName("Quantity")]
        public int cartQty { get; set; }
        [DisplayName("Total Price")]
        public double productTotalPrice { get; set; }

        public string productImage { get; set; }
    }
}
