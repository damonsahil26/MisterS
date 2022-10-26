using MisterS.Services.ShoppingCartAPI.Models.DTO;

namespace MisterS.Services.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
        public Task<CartDto> GetCartByUserId(string userId);

        public Task<CartDto> CreateUpdateCart(CartDto cartDto);

        public Task<bool> RemoveFromCart(int cardDetailsId);

        public Task<bool> ClearCart(string userId);
    }
}
