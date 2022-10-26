using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisterS.Services.ShoppingCartAPI.Models.DTO
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }
        public virtual CartHeaderDto? CartHeaderDto { get; set; }

        public int ProductId { get; set; }
        public virtual ProductDto? Product { get; set; }

        public int Count { get; set; }
    }
}
