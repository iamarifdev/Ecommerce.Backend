using Ecommerce.Backend.Entities;
using MongoDB.Entities;

namespace Ecommerce.Backend.Common.DTO
{
  public class UserRole
  {
    public UserRole()
    {

    }
    public UserRole(One<Role> rolRef)
    {
      var role = rolRef.ToEntity();
      ID = role.ID;
      Name = role.Name;
    }
    public string ID { get; set; }
    public string Name { get; set; }
  }
  public class AuthUserDto
  {
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNo { get; set; }
    public UserRole Role { get; set; }
    public string UserId { get; set; }
    public string FullName { get; set; }
    public string AvatarUrl { get; set; } = null;
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}