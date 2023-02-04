using SAMSUNG_4_YOU.Models;

namespace SAMSUNG_4_YOU.Repository
{
    public class Repository
    {
        private readonly Samsung4YouContext _db;
        public Repository(Samsung4YouContext db)
        {
            _db = db;
        }
    }
}
