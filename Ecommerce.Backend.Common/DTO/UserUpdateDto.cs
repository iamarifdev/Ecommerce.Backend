using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class UserUpdateDto
  {
    public string RoleId { get; set; }
    public string Username { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserPhoneNumberDto> PhoneNumbers { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNo { get; set; }
    public IEnumerable<UserAddressDto> Addresses { get; set; }
    public string Remarks { get; set; }
    public bool IsEnabled { get; set; } = true;
  }
}