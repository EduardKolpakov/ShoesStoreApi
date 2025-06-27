using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Request;

namespace ShoesStoreApi.Interface
{
    public interface IUserService
    {
        public Task<IActionResult> Login(string login, string password);
        public Task<IActionResult> Register(RegReq us);
        public Task<IActionResult> UpdateProfile(RegReq us, int id);
    }
}
