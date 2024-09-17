using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCMoneyTransfer.Data.AuthModels
{
    public class RegisterVM
    {
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;


    }

    public class LoginVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CreateRole
    {
        public string Name { get; set; }
    }

    public class ResetPasswordModel
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int UserId { get; set; }
    }

    public class Users
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }

    public class UnRegisterUser
    {
        public string UserId { get; set; } = string.Empty;
    }

}
