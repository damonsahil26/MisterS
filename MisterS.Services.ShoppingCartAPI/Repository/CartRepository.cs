using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MisterS.Services.ShoppingCartAPI.DbContexts;
using MisterS.Services.ShoppingCartAPI.Models;
using MisterS.Services.ShoppingCartAPI.Models.DTO;

namespace MisterS.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CartRepository(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
            cartFromDb.CouponCode = couponCode;
            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var headerFromDb = await _dbContext.CartHeaders
                .FirstOrDefaultAsync(ch => ch.UserId == userId);

            if (headerFromDb != null)
            {
                var cardDetailsFromDb = _dbContext.CartDetails.Where(cd => cd.CartHeaderId == headerFromDb.CartHeaderId);
                _dbContext.CartDetails.RemoveRange(cardDetailsFromDb);
                _dbContext.Remove(headerFromDb);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            try
            {
                var cart = _mapper.Map<CartDto, Cart>(cartDto);

                // check if product exists in cart, if not create it
                var prodInDb = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

                if (prodInDb == null)
                {
                    _dbContext.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                    await _dbContext.SaveChangesAsync();
                }

                // Check if header is null, if null create a cartHeader

                var cartHeaderInDb = await _dbContext.CartHeaders
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ch => ch.UserId == cartDto.CartHeader.UserId);

                if (cartHeaderInDb == null)
                {
                    _dbContext.CartHeaders.Add(cart.CartHeader);
                    await _dbContext.SaveChangesAsync();

                    cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    var cartDetailsFromDb = await _dbContext.CartDetails
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId
                        && x.CartHeaderId == cartHeaderInDb.CartHeaderId);

                    if (cartDetailsFromDb == null)
                    {
                        cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderInDb.CartHeaderId;
                        cart.CartDetails.FirstOrDefault().Product = null;
                        _dbContext.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        cart.CartDetails.FirstOrDefault().Product = null;
                        cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                        _dbContext.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                        await _dbContext.SaveChangesAsync();
                    }
                }

                return _mapper.Map<Cart, CartDto>(cart);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            var cartHeaderFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);

            if (cartHeaderFromDb != null)
            {
                var cardDetailsFromDb = _dbContext.CartDetails
                    .Where(cd => cd.CartHeaderId == cartHeaderFromDb.CartHeaderId)
                    .Include(ch => ch.Product);

                if (cardDetailsFromDb != null)
                {
                    return new CartDto
                    {
                        CartHeader = _mapper.Map<CartHeader, CartHeaderDto>(cartHeaderFromDb),
                        CartDetails = _mapper.Map<IEnumerable<CartDetails>, IEnumerable<CartDetailsDto>>(cardDetailsFromDb)
                    };
                }
            }

            return new CartDto();
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var cartFromDb = await _dbContext.CartHeaders.FirstOrDefaultAsync(ch => ch.UserId == userId);
            cartFromDb.CouponCode = "";
            _dbContext.CartHeaders.Update(cartFromDb);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            var cartDetails = await _dbContext.CartDetails
                .FirstOrDefaultAsync(x => x.CartDetailsId == cartDetailsId);

            if (cartDetails != null)
            {
                var totalCartItems = _dbContext.CartDetails
                    .Where(x => x.CartHeaderId == cartDetails.CartHeaderId)
                    .Count();

                _dbContext.CartDetails.Remove(cartDetails);

                if (totalCartItems == 1)
                {
                    var cartHeaderToRemove = await _dbContext.CartHeaders
                        .FirstOrDefaultAsync(ch => ch.CartHeaderId == cartDetails.CartHeaderId);

                    if (cartHeaderToRemove != null)
                    {
                        _dbContext.CartHeaders.Remove(cartHeaderToRemove);
                    }
                }

                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
