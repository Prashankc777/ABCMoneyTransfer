using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCMoneyTransfer.Data.AuthModels
{
    public class ApplicationUserRole : IdentityRole
    { }

    public class ApplicationUser : IdentityUser { }
}
