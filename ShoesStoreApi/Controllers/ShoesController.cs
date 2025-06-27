using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Interface;
using ShoesStoreApi.Request;

namespace ShoesStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoesController : ControllerBase
    {
        private readonly IShoesService _shoesService;

        public ShoesController(IShoesService shoesService)
        {
            _shoesService = shoesService;
        }

        [HttpPost("AddShoes")]
        [Authorize]
        public async Task<IActionResult> AddShoes([FromBody] NewShoesReq req)
        {
            return await _shoesService.AddShoes(req);
        }

        [HttpPut("UpdateShoes")]
        [Authorize]
        public async Task<IActionResult> UpdateShoes([FromBody] NewShoesReq req)
        {
            return await _shoesService.UpdateShoes(req);
        }

        [HttpGet("GetShoes/{id}")]
        public async Task<IActionResult> GetShoes(int id)
        {
            return await _shoesService.GetShoes(id);
        }

        [HttpDelete("DeleteShoes/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShoes(int id)
        {
            return await _shoesService.DeleteShoes(id);
        }
        [HttpGet("GetAllShoes")]
        public async Task<IActionResult> GetAllShoes()
        {
            return await _shoesService.GetAllShoes();
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            return await _shoesService.GetAllCategories();
        }
    }
}
