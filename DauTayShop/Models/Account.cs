using System;
using System.Collections.Generic;

namespace DauTayShop.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Type { get; set; }
    }
}
