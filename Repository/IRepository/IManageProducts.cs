using SAMSUNG_4_YOU.Models;

namespace SAMSUNG_4_YOU.Repository.IRepository
{
    public interface IManageProductsRepository
    {
        IEnumerable<Product> GetProducts();
        bool AddProduct(SAMSUNG_4_YOU.ViewModels.ManageProducts product, IFormFile file);
        Product DetailsProduct(int productId);
        bool UpdateProduct(SAMSUNG_4_YOU.ViewModels.ManageProducts product, IFormFile file);
        bool DeleteProduct(int productId);
    }
}
