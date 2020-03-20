namespace Ecommerce.Backend.Common.DTO
{
  public class CustomerListItemDto
  {
    public string ID { get; set; }
    public string PhoneNo { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AvatarUrl { get; set; }
    public int ProfileCompleteness { get; set; }
  }
}
