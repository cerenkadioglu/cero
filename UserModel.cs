using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class UserModel
    {
        public int Id { get; set; } // Birincil anahtar
        public string Username { get; set; } // Kullanıcı adı
        public string Password { get; set; } // Şifre
        public string Email { get; set; }    // Email

    
    }
}