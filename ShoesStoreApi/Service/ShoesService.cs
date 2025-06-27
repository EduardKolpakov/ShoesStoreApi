using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesStoreApi.DBModel;
using ShoesStoreApi.Interface;
using ShoesStoreApi.Model;
using ShoesStoreApi.Request;
using System.Diagnostics;

namespace ShoesStoreApi.Service
{
    public class ShoesService : IShoesService
    {
        private readonly ContextDb _context;

        public ShoesService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddShoes(NewShoesReq req)
        {
            if (!IsValidRequest(req, out var errorMessage))
                return new BadRequestObjectResult(errorMessage);

            var shoes = new Shoes
            {
                Name = req.Name.Trim(),
                Description = req.Description.Trim(),
                CategoryId = req.CategoryId,
                Sizes = req.Sizes.Trim(),
                ImageUrl = req.ImageUrl?.Trim(),
                Price = req.Price
            };

            _context.Shoes.Add(shoes);
            await _context.SaveChangesAsync();

            return new OkObjectResult(shoes);
        }

        public async Task<IActionResult> UpdateShoes(NewShoesReq req)
        {
            if (!IsValidRequest(req, out var errorMessage))
                return new BadRequestObjectResult(errorMessage);

            var existing = await _context.Shoes
                .FirstOrDefaultAsync(s => s.Name == req.Name && s.CategoryId == req.CategoryId);

            if (existing == null)
                return new NotFoundObjectResult("Обувь не найдена");

            existing.Description = req.Description.Trim();
            existing.Sizes = req.Sizes.Trim();
            existing.ImageUrl = req.ImageUrl?.Trim();
            existing.Price = req.Price;

            await _context.SaveChangesAsync();

            return new OkObjectResult(existing);
        }

        public async Task<IActionResult> GetShoes(int id)
        {
            var shoes = await _context.Shoes
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (shoes == null)
                return new NotFoundObjectResult("Обувь не найдена");

            return new OkObjectResult(shoes);
        }

        public async Task<IActionResult> DeleteShoes(int id)
        {
            var shoes = await _context.Shoes.FindAsync(id);
            if (shoes == null)
                return new NotFoundObjectResult("Обувь не найдена");

            _context.Shoes.Remove(shoes);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Обувь удалена");
        }

        private bool IsValidRequest(NewShoesReq req, out string error)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
            {
                error = "Название не может быть пустым";
                return false;
            }

            if (string.IsNullOrWhiteSpace(req.Description))
            {
                error = "Описание не может быть пустым";
                return false;
            }

            if (string.IsNullOrWhiteSpace(req.Sizes))
            {
                error = "Размеры не указаны";
                return false;
            }

            if (req.Price <= 0)
            {
                error = "Цена должна быть больше 0";
                return false;
            }

            if (string.IsNullOrWhiteSpace(req.ImageUrl))
            {
                error = "Ссылка на изображение не указана";
                return false;
            }

            error = null;
            return true;
        }

        public async Task<IActionResult> GetAllShoes()
        {
            var shoes = await _context.Shoes.ToListAsync();
            return new OkObjectResult(new {
            shoes = shoes
            });
        }

        public async Task<IActionResult> GetAllCategories()
        {
            var cats = await _context.Category.ToListAsync();
            return new OkObjectResult(new
            {
                categories = cats
            });
        }

    }
}
