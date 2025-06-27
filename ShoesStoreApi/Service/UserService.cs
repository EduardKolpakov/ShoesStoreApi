using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShoesStoreApi.DBModel;
using ShoesStoreApi.Interface;
using ShoesStoreApi.Model;
using ShoesStoreApi.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShoesStoreApi.Service
{
    public class UserService : IUserService
    {
        private readonly ContextDb _context;
        private readonly IConfiguration _config;

        public UserService(ContextDb context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IActionResult> Register(RegReq req)
        {
            if (string.IsNullOrWhiteSpace(req.login) ||
                string.IsNullOrWhiteSpace(req.password) ||
                string.IsNullOrWhiteSpace(req.email) ||
                string.IsNullOrWhiteSpace(req.Name))
            {
                return new BadRequestObjectResult("Все поля (login, password, email, name) обязательны");
            }

            if (await _context.User.AnyAsync(u => u.Login == req.login))
                return new BadRequestObjectResult("Пользователь с таким логином уже существует");

            var user = new User
            {
                Login = req.login,
                Password = req.password,
                Email = req.email,
                UserImage = string.IsNullOrWhiteSpace(req.Image) ? null : req.Image,
                Name = req.Name,
                Role = "user"
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Регистрация успешна");
        }


        public async Task<IActionResult> Login(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                return new BadRequestObjectResult("Логин и пароль обязательны");

            var user = await _context.User.FirstOrDefaultAsync(u => u.Login == login);
            if (user == null || password != user.Password)
                return new UnauthorizedObjectResult("Неверный логин или пароль");

            var token = GenerateJwtToken(user);
            return new OkObjectResult(new { token });
        }


        public async Task<IActionResult> UpdateProfile(RegReq req, int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null) return new NotFoundResult();

            user.Email = req.email;
            user.UserImage = req.Image;
            user.Name = req.Name;

            if (!string.IsNullOrEmpty(req.password))
                user.Password = req.password;

            await _context.SaveChangesAsync();
            return new OkObjectResult("Профиль обновлён");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Login),
        new Claim("userId", user.Id.ToString()),
        new Claim("email", user.Email ?? ""),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
