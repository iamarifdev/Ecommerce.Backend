using AutoMapper;
using Ecommerce.Backend.Common.DTO;
using Ecommerce.Backend.Entities;

namespace Ecommerce.Backend.API.AutoMappingProfiles
{
    public class RoleMappingProfile: Profile
  {
    public RoleMappingProfile()
    {
      CreateMap<RoleAddDto, Role>();
    }
  }
}