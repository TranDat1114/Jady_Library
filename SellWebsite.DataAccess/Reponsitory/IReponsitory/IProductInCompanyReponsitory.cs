using SellWebsite.Models.Models;

namespace SellWebsite.DataAccess.Reponsitory.IReponsitory
{
    public interface IProductInCompanyReponsitory : IReponsitory<ProductInCompany>
    {

        void Update(ProductInCompany productInCompany);

    }
}
