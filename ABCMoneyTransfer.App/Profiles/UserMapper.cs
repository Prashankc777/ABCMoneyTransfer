using ABCMoneyTransfer.Data.DTOs;
using ABCMoneyTransfer.Data.Entities;
using AutoMapper;

namespace ABCMoneyTransfer.App.Profiles
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserInsertDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
