using HomeBuddy.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartItems(int userId)
        {
            var (cartItems, subtotal) = await _cartService.GetCartItemsAsync(userId);

            return Ok(new
            {
                CartItems = cartItems,
                Subtotal = subtotal
            });
        }
    }
}
