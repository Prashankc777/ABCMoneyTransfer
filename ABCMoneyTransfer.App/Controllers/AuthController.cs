using ABCMoneyTransfer.Data.AuthModels;
using ABCMoneyTransfer.Data.Exceptions;
using ABCMoneyTransfer.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Exception = System.Exception;

namespace ABCMoneyTransfer.App.Controllers
{
    public class AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
        : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet , Route("user-management")]
        public IActionResult UserManagement()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginVM model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.Username))
                    throw new GeneralException("The Username cannot be empty.");

                if (string.IsNullOrEmpty(model.Password))
                    throw new GeneralException("The Password cannot be empty.");

                var user = await authRepository.GetUserByUsername(model.Username);

                if (user == null)
                {
                    logger.LogError($"Error! Failed to retrieve user with username {model.Username}.");
                    throw new GeneralException("The Username or Password is Wrong.");
                }

                var result = await authRepository.Login(model);

                if (!result)
                {
                    logger.LogError($"Error! Failed to login with credentials. Username {model.Username} & Password {model.Password}");
                    throw new GeneralException("The Username or Password is Wrong.");
                }
                
                return Ok("/");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet , Authorize , Route("api/auth/users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await authRepository.GetUsers();
                return Ok(users);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost , Route("api/auth/unregister") , Authorize]
        public async Task<IActionResult> RemoveUser([FromBody] UnRegisterUser registerUser)
        {
            try
            {
                if (string.IsNullOrEmpty(registerUser.UserId))
                {
                    throw new GeneralException("Invalid User");
                }
                await authRepository.UnRegisterUser(registerUser.UserId);
                return Ok("User has been deleted");
            }
            catch (Exception e)
            {
                logger.LogError( e , nameof(RemoveUser));
                return BadRequest(e.Message);
            }

        }

        [HttpGet , Route("api/auth/roles"), Authorize]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var data = await authRepository.GetRoles();
                return Ok(data);
            }
            catch (Exception e)
            {
                logger.LogError(e, nameof(RemoveUser));
                return BadRequest(e.Message);
            }
        }

        [HttpPost, Route("api/auth/register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterVM registerVm)
        {
            try
            {
                await authRepository.Register(registerVm);
                return Ok($"User {registerVm.Name} has been registered");
            }
            catch (Exception e)
            {
                logger.LogError(e , nameof(RegisterUser));
                return BadRequest(e.Message);
            }
        }
    }
}
