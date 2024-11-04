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

        [HttpPost("{userId}/{serviceId}/{quantity}")]
        public async Task<IActionResult> GetCartItems(int userId, int serviceId, int quantity)
        {
            var rs = await _cartService.AddToCartAsync(userId, serviceId, quantity);

            return Ok();
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

        [HttpDelete("{cartId}")]
        public async Task<IActionResult> RemoveCartItem(int cartId)
        {
            var result = await _cartService.RemoveCartItemAsync(cartId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent(); 
        }
    }
}
