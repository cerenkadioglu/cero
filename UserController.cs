using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

       [HttpPost("register")]
public async Task<IActionResult> Register([FromBody] Models.UserModel user)
{
    // Kullanıcı adı daha önce alındıysa hata döndür
    if (await _context.Users.AnyAsync(u => u.Username == user.Username))
        return BadRequest(new { message = "Bu kullanıcı adı zaten alınmış!" });

    // E-posta daha önce kullanıldıysa hata döndür
    if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        return BadRequest(new { message = "Bu e-posta zaten kullanılıyor!" });

    try
    {
        // Kullanıcıyı veritabanına ekle
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Kayıt başarılı!" });
    }
    catch (Exception ex)
    {
        // Hata durumunda 500 kodu döndür
        return StatusCode(500, new { message = "Bir hata oluştu!", error = ex.Message });
    }
} 


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.UserModel loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre!" });

            return Ok(new { message = "Giriş başarılı!" });
        }
    }

    public class UserModel
    {
        public int Id { get; set; } // Birincil anahtar
        public string Username { get; set; } // Kullanıcı adı
        public string Password { get; set; } // Şifre
        public string Email { get; set; }    // E-posta
    }
}
