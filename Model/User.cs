using System;
using System.Collections.Generic;

namespace haber1.Model
{
    public partial class User
    {
        public string Password { get; set; } = null!;
        public int UserType { get; set; }
        public string Username { get; set; } = null!;
        public int UserId { get; set; }
        public string? Fullname { get; set; }
    }
}
