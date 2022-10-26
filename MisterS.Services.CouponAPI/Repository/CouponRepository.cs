using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MisterS.Services.CouponAPI.DbContexts;
using MisterS.Services.CouponAPI.Models.DTO;
using MisterS.Services.CouponAPI.Repository.IRepository;

namespace MisterS.Services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetCouponFromCouponCode(string couponCode)
        {
            try
            {
                var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);

                if (coupon != null)
                {
                    return _mapper.Map<CouponDto>(coupon);
                }

                return new CouponDto();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
