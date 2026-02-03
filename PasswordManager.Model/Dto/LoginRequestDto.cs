using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordManager.Model.Dto
{
    public class LoginRequestDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
