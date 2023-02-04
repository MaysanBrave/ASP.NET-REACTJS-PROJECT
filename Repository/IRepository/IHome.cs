using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Repository.IRepository
{
    public interface IHomeRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<CustomerCartItems> GetCartItems();
        IEnumerable<Order> MyOrders();
        IEnumerable<Product> OrderDetails(int orderId);
        bool Checkout();
        string AddToCart(int productId, int productQty);
        bool RemoveFromCart(int cartId);
        string DecreaseCartQuantity(int cartId);
        string IncreseCartQuantity(int cartId);
    }
}
