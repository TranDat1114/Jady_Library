﻿using SellWebsite.DataAccess.Data;
using SellWebsite.DataAccess.Reponsitory.IReponsitory;

namespace SellWebsite.DataAccess.Reponsitory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryReponsitory Category { get; private set; }
        public IProductReponsitory Product { get; private set; }
        public ICompanyReponsitory Company { get; private set; }
        public IShoppingCartReponsitory ShoppingCart { get; private set; }
        public IApplicationUserReponsitory ApplicationUser { get; private set; }
        public IOrderDetailReponsitory OrderDetail { get; private set; }
        public IOrderHeaderReponsitory OrderHeader { get; private set; }
        public IProductInCompanyReponsitory ProductInCompany { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryReponsitory(_db);
            Product = new ProductReponsitory(_db);
            Company = new CompanyReponsitory(_db);
            ShoppingCart = new ShoppingCartReponsitory(_db);
            ApplicationUser = new ApplicationUserReponsitory(_db);
            OrderDetail = new OrderDetailReponsitory(_db);
            OrderHeader = new OrderHeaderReponsitory(_db);
            ProductInCompany = new ProductInCompanyReponsitory(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
