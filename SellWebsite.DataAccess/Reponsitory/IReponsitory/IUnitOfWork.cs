namespace SellWebsite.DataAccess.Reponsitory.IReponsitory
{
    public interface IUnitOfWork
    {
        ICategoryReponsitory Category { get; }
        IShoppingCartReponsitory ShoppingCart { get; }
        IProductReponsitory Product { get; }
        ICompanyReponsitory Company { get; }
        IApplicationUserReponsitory ApplicationUser { get; }
        IOrderHeaderReponsitory OrderHeader { get; }
        IOrderDetailReponsitory OrderDetail { get; }
        IProductInCompanyReponsitory ProductInCompany { get; }
        void Save();
    }
}
