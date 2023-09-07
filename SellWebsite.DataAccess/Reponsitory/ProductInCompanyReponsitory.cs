using SellWebsite.DataAccess.Data;
using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;

namespace SellWebsite.DataAccess.Reponsitory
{
    public class ProductInCompanyReponsitory : Reponsitory<ProductInCompany>, IProductInCompanyReponsitory
    {
        private readonly ApplicationDbContext _db;
        public ProductInCompanyReponsitory(ApplicationDbContext db) : base(db)
        {

            _db = db;
        }

        public void Update(ProductInCompany company)
        {
            _db.ProductInCompanies.Update(company);
        }

    }
}
