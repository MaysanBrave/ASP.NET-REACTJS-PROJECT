using SAMSUNG_4_YOU.Models;
using SAMSUNG_4_YOU.Repository.IRepository;

namespace SAMSUNG_4_YOU.Repository
{
    public class ManageCategoriesRepository : Repository, IManageCategoriesRepository
    {
        private Samsung4YouContext _db;
        public ManageCategoriesRepository(Samsung4YouContext db):base(db)
        {
            _db = db;
        }


        public IEnumerable<Category> GetCategories()
        {
            try
            {
                var list = _db.Categories.ToList();
                return list;
            }
            catch (Exception)
            {
                return Enumerable.Empty<Category>();
            }
        }


        public bool AddCategory(Category category)
        {
            try
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Category DetailsCategory(int categoryId)
        {
            try
            {
                var category =  _db.Categories.FirstOrDefault(m => m.CategoryId == categoryId);
                if (category != null)
                {
                    return category;
                }
                return new Category();
            }
            catch (Exception)
            {
                return new Category();
            }
        }

         public bool UpdateCategory(Category category)
        {
            try
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                var context = _db.Categories.Find(categoryId);
                if (context != null)
                {
                    _db.Categories.Remove(context);
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
