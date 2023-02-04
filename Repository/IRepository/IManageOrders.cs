using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Repository.IRepository
{
    public interface IManageOrdersRepository
    {
        IEnumerable<Order> GetOrders();
        IEnumerable<Product> DetailsOrder(int orderId);
        bool UpdateOrder(ManageOrders order);
        bool DeleteOrder(int orderId);
    }
}
