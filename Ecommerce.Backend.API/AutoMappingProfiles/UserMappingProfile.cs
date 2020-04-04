using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.API.AutoMappingProfiles
{
    public class UserMappingProfile: Profile
  {
    public UserMappingProfile()
    {
      CreateMap<UserAddDto, User>().ForPath(m => m.RoleRef.ID, (opt) => opt.MapFrom(dest => dest.RoleId));
      CreateMap<UserUpdateDto, User>().ForPath(m => m.RoleRef.ID, (opt) => opt.MapFrom(dest => dest.RoleId));;
      CreateMap<UserAddressDto, UserAddress>();
      CreateMap<UserPhoneNumberDto, UserPhoneNumber>();
    }
  }
}