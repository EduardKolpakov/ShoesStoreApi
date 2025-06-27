using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Request;
using System.Threading.Tasks;

namespace ShoesStoreApi.Interface
{
    public interface ICartService
    {
        Task<IActionResult> AddToCart(CartItemRequest req, int userId);
        Task<IActionResult> GetCart(int userId);
        Task<IActionResult> RemoveFromCart(int cartItemId, int userId);
        Task<IActionResult> UpdateCartItem(int cartItemId, CartItemRequest req, int userId);
        Task<IActionResult> ClearCart(int userId);
    }
}
