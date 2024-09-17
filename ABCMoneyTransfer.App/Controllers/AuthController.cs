using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABCMoneyTransfer.App.Controllers
{
    public class AuthController : Controller
    {
        public AuthController()
        {
            
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
    }
}
