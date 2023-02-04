using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAMSUNG_4_YOU.ViewModels;
using SAMSUNG_4_YOU.Repository.IRepository;

namespace SAMSUNG_4_YOU.Controllers
{
    [Route("api/manageCategories")]
    [ApiController]
    public class ManageCategoriesController : ControllerBase
    {
        private readonly IManageCategoriesRepository _category;
        public ManageCategoriesController(IManageCategoriesRepository category)
        {
            _category=category;
        }

        [HttpGet("getAllCategories")]
        public IActionResult GetCategories()
        {
            var categories = _category.GetCategories();
            return Ok(new { categories = categories });
        }


        [HttpPost("addCategory")]
        public IActionResult AddCategory(ManageCategories category)
        {
            var model = new Models.Category() { 
                CategoryName=category.CategoryName
            };
            bool confirm=_category.AddCategory(model);
            return Ok(new { isCategoryAdd = confirm });
        }


        [HttpGet("getCategoryDetails")]
        public IActionResult DetailsCateogry([FromQuery] int categoryId)
        {      
            var category = _category.DetailsCategory(categoryId);
            return Ok(new { categoryDetails = category });
        }


        [HttpPut("updateCategory")]
        public IActionResult UpdateCategory(ManageCategories category)
        {
            var model = new  Models.Category() {
              CategoryId=category.CategoryId,
              CategoryName=category.CategoryName
            };
            bool confirm = _category.UpdateCategory(model);
            return Ok(new { isCategoryUpdated = confirm });
        }

        [HttpDelete("deleteCategory")]
        public IActionResult DeleteCategory([FromQuery] int categoryId)
        {
            bool confirm = _category.DeleteCategory(categoryId);
            return Ok(new { isCategoryDeleted = confirm });
        }
    }
}
