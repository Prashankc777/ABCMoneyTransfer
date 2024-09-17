using ABCMoneyTransfer.Data.DTOs;
using ABCMoneyTransfer.Data.Entities;
using ABCMoneyTransfer.Data.Exceptions;
using ABCMoneyTransfer.Data.Repositories;
using ABCMoneyTransfer.Data.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ABCMoneyTransfer.App.Controllers
{
    [Authorize]
    public class UserController(
        IUserRepository userRepository,
        IMapper mapper, ILogger<UserController> logger,
        IHttpContextAccessor httpContextAccessor)
        : Controller
    {
        [Route("user/list")]
        public IActionResult List()
        {
            return View();
        }

        [Route("user/new")]
        public IActionResult New()
        {
            return View();
        }


        [HttpPost , Route("api/user/insert")]
        public async Task<IActionResult> Insert([FromBody] UserInsertDto userInsertDto)
        {
            try
            {
                if (userInsertDto == null) throw new GeneralException("The User cannot be empty.");

                if (string.IsNullOrEmpty(userInsertDto.FirstName))
                {
                    throw new GeneralException("The First Name cannot be empty.");
                }

                if (string.IsNullOrEmpty(userInsertDto.FirstName))
                {
                    throw new GeneralException("The First Name cannot be empty.");

                }

                if (string.IsNullOrEmpty(userInsertDto.LastName))
                {
                    throw new GeneralException("The Last Name cannot be empty.");
                }

                if (string.IsNullOrEmpty(userInsertDto.Address))
                {
                    throw new GeneralException("The Address cannot be empty.");
                }

                if (userInsertDto.CountryId == 0)
                {
                    throw new GeneralException("The Country cannot be empty.");
                }


                Data.Entities.User mappedUser = mapper.Map<User>(userInsertDto);
                mappedUser.CreatedBy = GeneralUtility.GetUsernameFromClaim(httpContextAccessor);
                mappedUser.CreatedDate = GeneralUtility.GetCurrentNepaliDateTime();
                string insertedUserName = await userRepository.Insert(mappedUser);
                return Ok($"The user {insertedUserName} has been added");
            }
            catch (Exception ex)
            {
                logger.LogError(ex , $"{nameof(UserController)}, {nameof(Insert)}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("api/user/update")]
        public async Task<IActionResult> Update ([FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                if (userUpdateDto == null) throw new GeneralException("The User cannot be empty.");


                if (string.IsNullOrEmpty(userUpdateDto.FirstName))
                {
                    throw new GeneralException("The First Name cannot be empty.");
                }

                if (string.IsNullOrEmpty(userUpdateDto.FirstName))
                {
                    throw new GeneralException("The First Name cannot be empty.");

                }

                if (string.IsNullOrEmpty(userUpdateDto.LastName))
                {
                    throw new GeneralException("The Last Name cannot be empty.");
                }

                if (string.IsNullOrEmpty(userUpdateDto.Address))
                {
                    throw new GeneralException("The Address cannot be empty.");
                }

                if (userUpdateDto.CountryId == 0)
                {
                    throw new GeneralException("The Country cannot be empty.");
                }

                Data.Entities.User mappedUser = mapper.Map<User>(userUpdateDto);
                mappedUser.ModifiedBy = GeneralUtility.GetUsernameFromClaim(httpContextAccessor);
                mappedUser.ModifiedDate = GeneralUtility.GetCurrentNepaliDateTime();
                await userRepository.Update(mappedUser);

                return Ok($"The user  has been updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UserController)}, {nameof(Update)}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost, Route("api/user/delete")]
        public async Task<IActionResult> Delete([FromBody] UserDeleteDto userDeleteDto)
        {
            try
            {
                if (userDeleteDto == null) throw new GeneralException("The User cannot be empty.");
                
                
                
                await userRepository.Delete(userDeleteDto.Id);

                return Ok($"The user  has been updated");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{nameof(UserController)}, {nameof(Delete)}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet , Route("api/user/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Data.Entities.User user = await userRepository.GetById(id);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet , Route("api/users")]
        public async Task<IActionResult> Get()
        {
            try
            {
                IEnumerable<User> users = await userRepository.GetAll();
                IEnumerable<UserDto> userList = mapper.Map<IEnumerable<UserDto>>(users);
                return Ok(userList);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
