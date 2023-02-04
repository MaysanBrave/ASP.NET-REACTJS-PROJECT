using Microsoft.EntityFrameworkCore;
using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Repository
{
    public class ManageOrdersRepository : Repository, IManageOrdersRepository
    {
        private Samsung4YouContext _db;
        public ManageOrdersRepository(Samsung4YouContext db):base(db)
        {
            _db = db;
        }

        public IEnumerable<Order> GetOrders()
        {
            try
            {
                var list = _db.Orders.ToList();
                return list;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Order>();
            }
        }
        public IEnumerable<Product> DetailsOrder(int orderId)
        {
            try
            {
                var order_details = (from orders in _db.Orders
                                          join order_detail
                                          in _db.OrderDetails on orders.OrderId equals
                                          order_detail.FkOrderId
                                          join product in _db.Products on order_detail.FkProductId equals product.ProductId
                                          where orders.OrderId == orderId
                                          select new Product
                                          {
                                              ProductId = product.ProductId,
                                              ProductName = product.ProductName,
                                              ProductPrice = product.ProductPrice,
                                              ProductQty = order_detail.Qty,
                                              ProductImage = product.ProductImage,
                                          }).ToList();

                return order_details;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Product>();
            }
        }

         public bool UpdateOrder(ManageOrders order)
        {
            try
            { 
                var _contextOrder = _db.Orders.Find(order.OrderId);
                if (_contextOrder != null)
                {
                    _contextOrder.FullName = order.FullName;
                    _contextOrder.PhoneNumber = order.PhoneNumber;
                    _contextOrder.AddressDetail = order.AddressDetail;
                    _contextOrder.OrderStatus = order.OrderStatus;

                    if (order.OrderStatus == 3)
                    {
                        var result = (from orders in _db.Orders
                                      join order_detail
                                      in _db.OrderDetails on orders.OrderId equals
                                      order_detail.FkOrderId
                                      where orders.OrderId == order.OrderId
                                      select new
                                      {
                                          productId = order_detail.FkProductId,
                                          productQty = order_detail.Qty
                                      }).ToList();
                        foreach (var item in result)
                        {
                            var _contextProductQty = _db.Products.Where(x => x.ProductId == item.productId).FirstOrDefault();
                            if (_contextProductQty != null)
                            {
                                _contextProductQty.ProductQty = _contextProductQty.ProductQty + item.productQty;
                                _db.Update(_contextProductQty);
                                _db.SaveChanges();
                            }
                           
                        }


                    }

                    _db.Update(_contextOrder);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteOrder(int orderId)
        {
            try
            {
                var context = _db.Orders.Find(orderId);
                if (context != null)
                {
                    _db.Orders.Remove(context);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

    }
}
