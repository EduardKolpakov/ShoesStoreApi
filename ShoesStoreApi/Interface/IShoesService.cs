using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Request;

namespace ShoesStoreApi.Interface
{
    public interface IShoesService
    {
        public Task<IActionResult> AddShoes(NewShoesReq shoes);
        public Task<IActionResult> UpdateShoes(NewShoesReq shoes);
        public Task<IActionResult> GetShoes(int id);
        public Task<IActionResult> DeleteShoes(int id);
        public Task<IActionResult> GetAllShoes();
        public Task<IActionResult> GetAllCategories();
    }
}
