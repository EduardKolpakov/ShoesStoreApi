using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Request;

namespace ShoesStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;

            Console.WriteLine($"[UPLOAD] WebRootPath = {_env.WebRootPath}");
        }


        [HttpPost("shoes")]
        public async Task<IActionResult> UploadShoesImage([FromForm] FileUploadRequest file)
        {
            if (file == null || file.File.Length == 0)
                return BadRequest("Файл не загружен");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.File.FileName)}";
            var dirPath = Path.Combine(_env.WebRootPath, "images", "shoes");
            Directory.CreateDirectory(dirPath);

            var filePath = Path.Combine(dirPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.File.CopyToAsync(stream);
            }

            var relativePath = $"/images/shoes/{fileName}";
            return Ok(new { imageUrl = relativePath });
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> UploadUserAvatar([FromForm] FileUploadRequest file)
        {
            if (file == null || file.File.Length == 0)
                return BadRequest("Файл не загружен");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.File.FileName)}";
            var dirPath = Path.Combine(_env.WebRootPath, "images", "users");
            Directory.CreateDirectory(dirPath);

            var filePath = Path.Combine(dirPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.File.CopyToAsync(stream);
            }

            var relativePath = $"/images/users/{fileName}";
            return Ok(new { imageUrl = relativePath });
        }
    }
}