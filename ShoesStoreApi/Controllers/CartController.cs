using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Interface;
using ShoesStoreApi.Request;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoesStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            var userIdStr = User.FindFirst("userId")?.Value;
            return int.TryParse(userIdStr, out var id) ? id : 0;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest req)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            return await _cartService.AddToCart(req, userId);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            return await _cartService.GetCart(userId);
        }

        [HttpDelete("Delete/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            return await _cartService.RemoveFromCart(cartItemId, userId);
        }

        [HttpPut("Update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] CartItemRequest req)
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            return await _cartService.UpdateCartItem(cartItemId, req, userId);
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            if (userId == 0) return Unauthorized();

            return await _cartService.ClearCart(userId);
        }
    }
}
