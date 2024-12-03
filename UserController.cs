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

        // POST: api/user/register
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

        // POST: api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.UserModel loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre!" });

            return Ok(new { message = "Giriş başarılı!" });
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users); // Tüm kullanıcıları döndürür
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = "Kullanıcı bulunamadı!" });

            return Ok(user); // Kullanıcıyı ID ile döndürür
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
