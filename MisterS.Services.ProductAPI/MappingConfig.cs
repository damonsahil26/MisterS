using AutoMapper;
using MisterS.Services.ProductAPI.Models;
using MisterS.Services.ProductAPI.Models.DTO;

namespace MisterS.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
