// using System.Security.Cryptography;
// using System.Text;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using BookShop.Data;
// using BookShop.Data.Models;
// using BookShop.Shared.DTO.Requests;
// using Microsoft.EntityFrameworkCore;
//
// namespace BookShop.Controllers
// {
//     [Route("api/auth")]
//     [ApiController]
//     public class AuthController : ControllerBase
//     {
//         private readonly LibraryContext _context;
//         private readonly IConfiguration _configuration;
//
//         public AuthController(LibraryContext context, IConfiguration configuration)
//         {
//             _context = context;
//             _configuration = configuration;
//         }
//
//         [HttpPost("register")]
//         public async Task<IActionResult> Register(UserRegisterDTO request)
//         {
//             if (await _context.Users.AnyAsync(u => u.Email == request.Email))
//                 return BadRequest("Пользователь с таким email уже существует.");
//
//             CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);
//
//             var user = new User
//             {
//                 UserName = request.Username,
//                 Email = request.Email,
//                 PasswordHash = passwordHash,
//                 PasswordSalt = passwordSalt,
//                 CreatedAt = DateTime.UtcNow
//             };
//
//             _context.Users.Add(user);
//             await _context.SaveChangesAsync();
//
//             return Ok("Регистрация успешна!");
//         }
//
//         [HttpPost("login")]
//         public async Task<IActionResult> Login(UserLoginDTO request)
//         {
//             var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
//             if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
//                 return Unauthorized("Неверный email или пароль.");
//
//             var token = GenerateJwtToken(user);
//             return Ok(new { token });
//         }
//
//         private string GenerateJwtToken(User user)
//         {
//             var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
//             var tokenHandler = new JwtSecurityTokenHandler();
//             var tokenDescriptor = new SecurityTokenDescriptor
//             {
//                 Subject = new ClaimsIdentity(new[]
//                 {
//                     new Claim(ClaimTypes.Name, user.Id.ToString()),
//                     new Claim(ClaimTypes.Role, user.UserRoles.FirstOrDefault()?.Role.Name ?? "User")
//                 }),
//                 Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"])),
//                 Issuer = _configuration["JwtSettings:Issuer"],
//                 Audience = _configuration["JwtSettings:Audience"],
//                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
//             };
//
//             var token = tokenHandler.CreateToken(tokenDescriptor);
//             return tokenHandler.WriteToken(token);
//         }
//
//         private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
//         {
//             using var hmac = new HMACSHA256();
//             passwordSalt = Convert.ToBase64String(hmac.Key);
//             passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
//         }
//
//         private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
//         {
//             using var hmac = new HMACSHA256(Convert.FromBase64String(storedSalt));
//             var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
//             return computedHash == storedHash;
//         }
//     }
// }
