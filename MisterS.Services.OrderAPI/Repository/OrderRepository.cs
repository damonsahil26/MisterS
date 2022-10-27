using Microsoft.EntityFrameworkCore;
using MisterS.Services.OrderAPI.DbContexts;
using MisterS.Services.OrderAPI.Models;
using MisterS.Services.OrderAPI.Repository.IRepository;

namespace MisterS.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            try
            {
                await using var _db = new ApplicationDbContext(_dbContext);

                _db.OrderHeaders.Add(orderHeader);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool isPaymentSuccessful)
        {
            try
            {
                await using var _db= new ApplicationDbContext(_dbContext);

                if (isPaymentSuccessful)
                {
                    var orderHeader = await _db.OrderHeaders.FirstOrDefaultAsync(oh=>oh.OrderHeaderId == orderHeaderId);
                    if (orderHeader != null)
                    {
                        orderHeader.PaymentStatus = isPaymentSuccessful;
                        await _db.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
