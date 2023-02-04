using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Controllers
{
    [Route("api/manageOrders")]
    [ApiController]
    public class ManageOrdersController : ControllerBase
    {
        private readonly IManageOrdersRepository _order;
        public ManageOrdersController(IManageOrdersRepository order)
        {
            _order = order;
        }

        [HttpGet("getAllOrders")]
        public IActionResult GetOrders()
        {
            var orders = _order.GetOrders();
            return Ok(new { orders = orders });
        }


        //[HttpPost("addOrder")]
        //public IActionResult AddOrder(Order order)
        //{
        //    var model = new Models.Order()
        //    {
        //        OrderName = order.OrderName
        //    };
        //    bool confirm = _order.AddOrder(model);
        //    return Ok(new { isOrderAdd = confirm });
        //}


        [HttpGet("getOrderDetails")]
        public IActionResult DetailsCateogry([FromQuery] int orderId)
        {
            var order = _order.DetailsOrder(orderId);
            return Ok(new { orderDetails = order });
        }


        [HttpPut("updateOrder")]
        public IActionResult UpdateOrder(ManageOrders order)
        {
            bool confirm = _order.UpdateOrder(order);
            return Ok(new { isOrderUpdated = confirm });
        }

        [HttpDelete("deleteOrder")]
        public IActionResult DeleteOrder([FromQuery] int orderId)
        {
            bool confirm = _order.DeleteOrder(orderId);
            return Ok(new { isOrderDeleted = confirm });
        }
    }
}
