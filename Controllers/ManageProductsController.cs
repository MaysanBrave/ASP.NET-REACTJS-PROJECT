using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAMSUNG_4_YOU.Repository.IRepository;
using SAMSUNG_4_YOU.ViewModels;

namespace SAMSUNG_4_YOU.Controllers
{
    [Route("api/manageProducts")]
    [ApiController]
    public class ManageProductsController : ControllerBase
    {
        private readonly IManageProductsRepository _product;
        public ManageProductsController(IManageProductsRepository product)
        {
            _product = product;
        }

        [HttpGet("getAllProducts")]
        public IActionResult GetProducts()
        {
            var products = _product.GetProducts();
            return Ok(new { products = products });
        }


        [HttpPost("addProduct")]
        public IActionResult AddProduct([FromForm] ManageProducts product)
        {
            bool confirm = _product.AddProduct(product, product.ProductImage);
            return Ok(new { isProductAdd = confirm });
        }


        [HttpGet("getProductDetails")]
        public IActionResult DetailsCateogry([FromQuery] int productId)
        {
            var product = _product.DetailsProduct(productId);
            return Ok(new { productDetails = product });
        }


        [HttpPut("updateProduct")]
        public IActionResult UpdateProduct([FromForm] ManageProducts product)
        {
           
            bool confirm = _product.UpdateProduct(product,product.ProductImage);
            return Ok(new { isProductUpdated = confirm });
        }

        [HttpDelete("deleteProduct")]
        public IActionResult DeleteProduct([FromQuery] int productId)
        {
            bool confirm = _product.DeleteProduct(productId);
            return Ok(new { isProductDeleted = confirm });
        }
    }
}
