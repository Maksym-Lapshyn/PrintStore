using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintStore.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public bool IsBlocked { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public const string UserRole = "User";
        public const string ManagerRole = "Manager";
        public const string AdminRole = "Admin";
    }
}