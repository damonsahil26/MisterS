using MisterS.Services.OrderAPI.Models;

namespace MisterS.Services.OrderAPI.Repository.IRepository
{
    public interface IOrderRepository
    {
        public Task<bool> AddOrder(OrderHeader orderHeader);

        public Task UpdateOrderPaymentStatus(int orderHeaderId, bool isPaymentSuccessful);
    }
}
