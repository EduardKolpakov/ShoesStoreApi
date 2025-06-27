using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesStoreApi.Interface;
using ShoesStoreApi.Request;
using ShoesStoreApi.DBModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoesStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ContextDb _context;

        public UserController(IUserService userService, ContextDb context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegReq req)
        {
            return await _userService.Register(req);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            return await _userService.Login(loginRequest.Login, loginRequest.Password);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst("userId");
            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return BadRequest("Некорректный ID пользователя");

            var user = await _context.User.FindAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.Login,
                user.Email,
                user.Name,
                user.UserImage
            });
        }
    }
}
