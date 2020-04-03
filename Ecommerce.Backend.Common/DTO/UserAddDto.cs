using System.Collections.Generic;

namespace Ecommerce.Backend.Common.DTO
{
  public class PhoneNumberDto
  {
    public string PhoneNo { get; set; }
  }
  public class AddressDto
  {
    public string District { get; set; }
    public string Thana { get; set; }
    public string PostCode { get; set; }
    public string Description { get; set; }
  }
  public class UserAddDto
  {
    public string CompanyId { get; set; }
    public string RoleId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }
    public string ContactPerson { get; set; }
    public string ContactNo { get; set; }
    public IEnumerable<AddressDto> Addresses { get; set; }
    public string Remarks { get; set; }
    public bool IsEnabled { get; set; }
  }
}