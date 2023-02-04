using System;
using System.Collections.Generic;

namespace SAMSUNG_4_YOU.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
