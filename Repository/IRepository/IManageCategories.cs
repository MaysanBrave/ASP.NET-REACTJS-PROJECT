using SAMSUNG_4_YOU.Models;

namespace SAMSUNG_4_YOU.Repository.IRepository
{
    public interface IManageCategoriesRepository
    {
        IEnumerable<Category> GetCategories();
        bool AddCategory(Category category);
        Category DetailsCategory(int categoryId);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int categoryId);
    }
}
