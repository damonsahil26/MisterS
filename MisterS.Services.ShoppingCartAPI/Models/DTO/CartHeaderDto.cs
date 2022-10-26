using System.ComponentModel.DataAnnotations;

namespace MisterS.Services.ShoppingCartAPI.Models.DTO
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string CouponCode { get; set; } = string.Empty;
    }
}
