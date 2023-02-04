using System.ComponentModel.DataAnnotations;

namespace SAMSUNG_4_YOU.ViewModels
{
    public class ManageProducts
    {
        public int ProductId { get; set; }
        public int FkCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public double ProductPrice { get; set; }
        public int ProductQty { get; set; }
        public IFormFile ProductImage { get; set; } = null!;
        public string ProductDesc { get; set; } = null!;

    }
}
