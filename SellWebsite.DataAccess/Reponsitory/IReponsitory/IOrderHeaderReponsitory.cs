using SellWebsite.Models.Models;

namespace SellWebsite.DataAccess.Reponsitory.IReponsitory
{
    public interface IOrderHeaderReponsitory : IReponsitory<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int id, string orderStatus);
        //void UpdatePaypalPaymentId(int id, string paymentId, string payerId);


    }
}
