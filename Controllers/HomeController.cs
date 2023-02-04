using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository _home;

        private readonly IAuthRepository _auth;
        public HomeController(IHomeRepository home,IAuthRepository auth)
        {
            _home = home;
            _auth = auth;
        }

        [HttpGet("getAllProducts")]
        public IActionResult GetProducts()
        {
            var products = _home.GetProducts();
            return Ok(new { products = products });
        }

        [HttpGet("getCartItems")]
        public IActionResult GetCartItems()
        {
            int userId=_auth.getUserId();
            if (userId != 0)
            {
                try
                {
                   return Ok(new { getCartItems = _home.GetCartItems() });
                }
                catch (Exception)
                {
                    return Ok(new {exception="Something went wrong, please try again later" });
                }
               
            }
            return Ok(new { unauthorizedAccess=true });
        }



        [HttpGet("addToCart")]
        public IActionResult addToCart([FromQuery] int productId, int productQty)
        {

            int userId = _auth.getUserId();
            if (userId != 0)
            {
                string response = _home.AddToCart(productId, productQty);
                if (response == "false")
                {
                    return Ok(new { exception = "Something went wrong, please try again later" });
                }
                if(response== "Hi, item is out of stock" || response== "Product is added into cart")
                {
                    return Ok(new { message = response });
                }
                
            }
            return Ok(new { unauthorizedAccess = true });
        }

        [HttpGet("removeFromCart")]
        public IActionResult RemoveItemFromCart([FromQuery] int cartId)
        {
            int userId = _auth.getUserId();
            if (userId != 0)
            {
                bool response = _home.RemoveFromCart(cartId);
                return Ok(new {isItemRemoved=response});
            }
            return Ok(new { unauthorizedAccess = true });
        }


        [HttpGet("increaseCartQuantity")]
        public IActionResult IncreseCartQuantity([FromQuery] int cartId)
        {

            int userId = _auth.getUserId();
            if (userId != 0)
            {
                string response = _home.IncreseCartQuantity(cartId);
                if (response == "false")
                {
                    return Ok(new { exception = "Something went wrong, please try again later" });
                }
                if (response == "Hi, item is out of stock" || response == "true")
                {
                    return Ok(new { message = response });
                }

            }
            return Ok(new { unauthorizedAccess = true });
        }


        [HttpGet("decreaseCartQuantity")]
        public IActionResult DecreaseCartQuantity([FromQuery] int cartId)
        {

            int userId = _auth.getUserId();
            if (userId != 0)
            {
                string response = _home.DecreaseCartQuantity(cartId);
                if (response == "false")
                {
                    return Ok(new { exception = "Something went wrong, please try again later" });
                }
                if (response == "Hi, quantity can not be decrease, minimum should be 1 for product" || response == "true")
                {
                    return Ok(new { message = response });
                }

            }
            return Ok(new { unauthorizedAccess = true });
        }


        [HttpGet("checkout")]
        public IActionResult Checkout()
        {

            int userId = _auth.getUserId();
            if (userId != 0)
            {
                bool response = _home.Checkout();
                if (!response)
                {
                    return Ok(new { exception = "Something went wrong, please try again later" });
                }
                else
                {
                    return Ok(new { message = "Your order has been confirmed successfully" });
                }

            }
            return Ok(new { unauthorizedAccess = true });
        }



        [HttpGet("getMyOrders")]
        public IActionResult GetMyOrders()
        {
            int userId = _auth.getUserId();
            if (userId != 0)
            {
                try
                {
                    return Ok(new { myOrders = _home.MyOrders() });
                }
                catch (Exception)
                {
                    return Ok(new { exception = "Something went wrong, please try again later" });
                }

            }
            return Ok(new { unauthorizedAccess = true });
        }



        [HttpGet("getMyOrderDetails")]
        public IActionResult GetMyOrdersDetails([FromQuery] int orderId)
        {
            int userId = _auth.getUserId();
            if (userId != 0)
            {
                try
                {
                   return Ok(new { myOrdersDetails = _home.OrderDetails(orderId) });
                }
                catch (Exception)
                {
                    return Ok(new { exception = "Something went wrong, please try again later" });
                }

            }
            return Ok(new { unauthorizedAccess = true });
        }




    }
}
