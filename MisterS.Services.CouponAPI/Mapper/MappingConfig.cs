using AutoMapper;
using MisterS.Services.CouponAPI.Models;
using MisterS.Services.CouponAPI.Models.DTO;

namespace MisterS.Services.CouponAPI.Mapper
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
