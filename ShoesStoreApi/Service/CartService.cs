using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStoreApi.DBModel;
using ShoesStoreApi.Interface;
using ShoesStoreApi.Model;
using ShoesStoreApi.Request;
using System.Linq;
using System.Threading.Tasks;

namespace ShoesStoreApi.Service
{
    public class CartService : ICartService
    {
        private readonly ContextDb _context;

        public CartService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddToCart(CartItemRequest req, int userId)
        {
            if (req.Quantity <= 0)
                return new BadRequestObjectResult("Количество должно быть больше 0");

            if (string.IsNullOrWhiteSpace(req.Size))
                return new BadRequestObjectResult("Размер обязателен");

            var existing = await _context.CartItem
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ShoesId == req.ShoesId && c.Size == req.Size);

            if (existing != null)
            {
                existing.Quantity += req.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ShoesId = req.ShoesId,
                    Size = req.Size,
                    Quantity = req.Quantity
                };
                await _context.CartItem.AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult("Добавлено в корзину");
        }

        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _context.CartItem.Where(x => x.UserId == userId).ToListAsync();
            return new OkObjectResult(new
            {
                items = cart
            });
        }

        public async Task<IActionResult> RemoveFromCart(int cartItemId, int userId)
        {
            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
                return new NotFoundObjectResult("Элемент корзины не найден");

            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Элемент удалён из корзины");
        }

        public async Task<IActionResult> UpdateCartItem(int cartItemId, CartItemRequest req, int userId)
        {
            if (req.Quantity <= 0)
                return new BadRequestObjectResult("Количество должно быть больше 0");

            if (string.IsNullOrWhiteSpace(req.Size))
                return new BadRequestObjectResult("Размер обязателен");

            var cartItem = await _context.CartItem
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
                return new NotFoundObjectResult("Элемент корзины не найден");

            cartItem.Size = req.Size;
            cartItem.Quantity = req.Quantity;

            await _context.SaveChangesAsync();

            return new OkObjectResult("Элемент корзины обновлён");
        }

        public async Task<IActionResult> ClearCart(int userId)
        {
            var items = _context.CartItem.Where(c => c.UserId == userId);
            _context.CartItem.RemoveRange(items);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Корзина очищена");
        }
    }
}
