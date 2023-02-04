using Microsoft.EntityFrameworkCore;
using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.Repository.IRepository;

namespace SAMSUNG_4_YOU.Repository
{
    public class ManageProductsRepository : Repository, IManageProductsRepository
    {
        private Samsung4YouContext _db;

        private readonly IWebHostEnvironment _hostEnvironment;
        public ManageProductsRepository(Samsung4YouContext db, IWebHostEnvironment hostEnvironment) : base(db)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
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


        public bool AddProduct(SAMSUNG_4_YOU.ViewModels.ManageProducts product, IFormFile file)
        {
            try
            {
                //save image to folder wwwroth
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extention = Path.GetExtension(file.FileName);

                var model = new Models.Product()
                {
                    FkCategoryId = product.FkCategoryId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductQty = product.ProductQty,
                    ProductDesc = product.ProductDesc,
                    ProductImage = fileName = Guid.NewGuid().ToString() + "_" + fileName + extention
                };

                string path = Path.Combine(wwwRootPath + "/images/uploads/productImages/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                _db.Products.Add(model);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Product DetailsProduct(int productId)
        {
            try
            {
                var product =  _db.Products.FirstOrDefault(m => m.ProductId == productId);
                if (product != null)
                {
                    return product;
                }
                return new Product();
            }
            catch (Exception)
            {
                return new Product();
            }
        }

         public bool UpdateProduct(SAMSUNG_4_YOU.ViewModels.ManageProducts product, IFormFile file)
        {
            try
            {
                //save image to folder wwwroth
                string wwwRootPath = _hostEnvironment.WebRootPath;
                var _context = _db.Products.Find(product.ProductId);
                if (_context != null)
                {
                   
                    var deleteImage = _context.ProductImage;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extention = Path.GetExtension(file.FileName);
                    _context.FkCategoryId = product.FkCategoryId;
                    _context.ProductName = product.ProductName;
                    _context.ProductPrice = product.ProductPrice;
                    _context.ProductQty = product.ProductQty;
                    _context.ProductDesc = product.ProductDesc;
                    _context.ProductImage = fileName = Guid.NewGuid().ToString() + "_" + fileName + extention;
                    string path = Path.Combine(wwwRootPath + "/images/uploads/productImages/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    _db.Update(_context);
                    _db.SaveChanges();
                    var deleteImagepath = Path.Combine(_hostEnvironment.ContentRootPath, wwwRootPath + "/images/uploads/productImages/", deleteImage);

                    if (System.IO.File.Exists(deleteImagepath))
                    {
                        System.IO.File.Delete(deleteImagepath);
                    }
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteProduct(int productId)
        {
            try
            {
                var context = _db.Products.Find(productId);
                if (context != null)
                {
                    var imagePath = context.ProductImage;
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    _db.Products.Remove(context);
                    _db.SaveChanges();

                    var deleteImagepath = Path.Combine(_hostEnvironment.ContentRootPath, wwwRootPath + "/images/uploads/productImages/", imagePath);
                    if (System.IO.File.Exists(deleteImagepath))
                    {
                        System.IO.File.Delete(deleteImagepath);
                    }
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
