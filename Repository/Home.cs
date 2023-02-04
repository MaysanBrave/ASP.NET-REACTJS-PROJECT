using Microsoft.EntityFrameworkCore;
using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Repository
{
    public class HomeRepository : Repository, IHomeRepository
    {
        private Samsung4YouContext _db;
        private IAuthRepository _auth;
        private IHttpContextAccessor _httpContext;
  
        public HomeRepository(Samsung4YouContext db,IAuthRepository auth, IHttpContextAccessor httpContext) : base(db)
        {
            _db = db;
            _auth= auth;
            _httpContext= httpContext;
        }

        public IEnumerable<Product> GetProducts()
        {
            try
            {
                var list = _db.Products.ToList();
                return list;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Product>();
            }
        }


        public IEnumerable<CustomerCartItems> GetCartItems()
        {
            var getUserId=_auth.getUserId();
            var result = (from products in _db.Products
                          join cart in _db.Carts
                          on products.ProductId equals cart.FkProductId
                          join customer in _db.Customers on cart.FkCustomerId equals customer.CustomerId
                          where customer.CustomerId == getUserId
                          select new CustomerCartItems
                          {
                              cartId = cart.CartId,
                              customerId = customer.CustomerId,
                              productId = products.ProductId,
                              productName = products.ProductName,
                              productImage=products.ProductImage,
                              productPrice = products.ProductPrice,
                              cartQty = cart.Qty,
                              productTotalPrice = cart.TotalPrice

                          }).ToList();
            return result;
        }

        public string AddToCart(int productId, int productQty)
        {
            try
            {
                int customerId = _auth.getUserId();
                //var customerEmail = _auth.getUserEmail();
                //if (customerId == 0)
                //{
                //    return "Customer is not login";
                //}
                var _contextProduct = _db.Products.Find(productId);
                if (_contextProduct.ProductQty < productQty)
                {
                    return "Hi, item is out of stock";
                }
                else
                {
                    var _contextCart = _db.Carts.Where(x => x.FkProductId == productId && x.FkCustomerId == customerId).FirstOrDefault();
                    if (_contextCart != null)
                    {
                        _contextCart.TotalPrice = _contextCart.TotalPrice + (_contextProduct.ProductPrice * productQty);
                        _contextCart.Qty = _contextCart.Qty + productQty;
                        _db.Carts.Update(_contextCart);
                        _contextProduct.ProductQty = _contextProduct.ProductQty - productQty;
                        _db.Products.Update(_contextProduct);
                        _db.SaveChanges();
                    }
                    else
                    {
                        var data = new Cart();
                        data.FkCustomerId = customerId;
                        data.FkProductId = productId;
                        data.Qty = productQty;
                        data.TotalPrice = _contextProduct.ProductPrice * productQty;
                        _db.Carts.Add(data);
                        _contextProduct.ProductQty = _contextProduct.ProductQty - productQty;
                        _db.Products.Update(_contextProduct);
                        _db.SaveChanges();
                    }
                    return "Product is added into cart";
                }
            }
            catch (Exception)
            {
                return "false";
            }

        }

        public bool RemoveFromCart(int cartId)
        {
            try
            {
                var _contextCart = _db.Carts.Find(cartId);
                var _contextProduct = _db.Products.Find(_contextCart.FkProductId);
                _contextProduct.ProductQty = _contextProduct.ProductQty + _contextCart.Qty;
                _db.Products.Update(_contextProduct);
                _db.Carts.Remove(_contextCart);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public string IncreseCartQuantity(int cartId)
        {
            try
            {
                var _contextCart = _db.Carts.Find(cartId);
                var _contextProduct = _db.Products.Find(_contextCart.FkProductId);
                if (_contextProduct.ProductQty > 0)
                {
                    _contextProduct.ProductQty = _contextProduct.ProductQty - 1;
                    _contextCart.Qty = _contextCart.Qty + 1;
                    _contextCart.TotalPrice = _contextCart.TotalPrice + _contextProduct.ProductPrice;
                    _db.Products.Update(_contextProduct);
                    _db.Carts.Update(_contextCart);
                    _db.SaveChanges();
                }
                else
                {
                    return "Hi, item is out of stock";
                }
            }
            catch (Exception)
            {
                return "false";
            }
            return "true";
        }

        public string DecreaseCartQuantity(int cartId)
        {
            try
            {
                var _contextCart = _db.Carts.Find(cartId);
                var _contextProduct = _db.Products.Find(_contextCart.FkProductId);
                if (_contextCart.Qty < 2)
                {
                    return "Hi, quantity can not be decrease, minimum should be 1 for product";
                }
                else
                {
                    _contextProduct.ProductQty = _contextProduct.ProductQty + 1;
                    _contextCart.Qty = _contextCart.Qty - 1;
                    _contextCart.TotalPrice = _contextCart.TotalPrice - _contextProduct.ProductPrice;
                    _db.Products.Update(_contextProduct);
                    _db.Carts.Update(_contextCart);
                    _db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return "false";
            }
            return "true";
        }


        public bool Checkout()
        {
            try
            {
                var getUserId = _auth.getUserId();
                var getUser = _auth.getCustomerDetails(getUserId);
                double subTotals = _db.Carts.Where(c => c.FkCustomerId == getUserId)
                .Select(i => Convert.ToDouble(i.TotalPrice)).Sum();
                
                var order = new Order() {
                FkCustomerId = getUserId,
                FullName= getUser.CustomerName,
                PhoneNumber= getUser.CustomerPhone,
                AddressDetail= getUser.CustomerAddress,
                TotalPrice = subTotals,
                OrderDate = DateTime.Now.ToString(),
                OrderStatus = 0

            };   
                _db.Orders.Add(order);
                _db.SaveChanges();
                var _contextCart = _db.Carts.Where(x => x.FkCustomerId == getUserId).ToList();
                foreach (var items in _contextCart)
                {
                    var itemPrice = _db.Products.Find(items.FkProductId);
                    var orderDetail = new OrderDetail();
                    orderDetail.FkOrderId = order.OrderId;
                    orderDetail.FkProductId = items.FkProductId;
                    orderDetail.Qty = items.Qty;
                    orderDetail.Price = itemPrice.ProductPrice;
                    _db.OrderDetails.Add(orderDetail);
                    _db.SaveChanges();
                    _db.Carts.Remove(items);
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public IEnumerable<Order> MyOrders()
        {
            var getUserId = _auth.getUserId();
            var result = _db.Orders.Where(x => x.FkCustomerId == getUserId).OrderByDescending(x => x.OrderId).ToList();
            return result;
        }

        public IEnumerable<Product> OrderDetails(int orderId)
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

    }
}
