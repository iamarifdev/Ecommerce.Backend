using System;

namespace Ecommerce.Backend.Common.DTO
{
  public class ManufactureDetailDto
  {
    public ManufactureDetailDto() { }
    public ManufactureDetailDto(string modelNo, DateTime releaseDate)
    {
      ModelNo = modelNo;
      ReleaseDate = releaseDate;
    }
    public string ModelNo { get; set; }
    public DateTime ReleaseDate { get; set; }
  }
}