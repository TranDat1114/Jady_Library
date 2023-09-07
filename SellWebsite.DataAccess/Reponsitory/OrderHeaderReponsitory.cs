using SellWebsite.DataAccess.Data;
using SellWebsite.DataAccess.Reponsitory.IReponsitory;
using SellWebsite.Models.Models;

namespace SellWebsite.DataAccess.Reponsitory
{
    public class OrderHeaderReponsitory : Reponsitory<OrderHeader>, IOrderHeaderReponsitory
    {
        private ApplicationDbContext _db;
        public OrderHeaderReponsitory(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(p => p.OrderHeaderId == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
            }

        }

    }
}
