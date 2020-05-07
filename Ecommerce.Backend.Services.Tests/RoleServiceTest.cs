using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Backend.Common.Helpers;
using Ecommerce.Backend.Common.Models;
using Ecommerce.Backend.Entities;
using Ecommerce.Backend.Services.Abstractions;
using Moq;
using Xunit;

namespace Ecommerce.Backend.Services.Tests
{
  public class RoleServiceTest
  {
    private const string _superAdminId = "5eb3f92920d7467aed27c7c2";
    private const string _adminId = "5eb3f92b20d7467aed27c7c3";
    private readonly List<Role> _roles;
    private readonly Mock<IRoleService> _mockRolerService;

    public RoleServiceTest()
    {
      _mockRolerService = new Mock<IRoleService>(MockBehavior.Strict);
      _roles = new List<Role>
      {
        new Role { ID = _superAdminId, Name = "Super Admin", Description = "Super Admin" },
        new Role { ID = _adminId, Name = "Admin", Description = "Administrator" },
      };
    }

    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(20)]
    public async Task Get_Paginated_Role_List_With_PageSize_Test(int pageSize)
    {
      // Arrange
      var pagedQuery = new PagedQuery { PageSize = pageSize };
      _mockRolerService.Setup(s => s.GetPaginatedList(It.IsAny<PagedQuery>())).ReturnsAsync(() => _roles.ToPagedList(_roles.Count));

      // Assert
      var pagedList = await _mockRolerService.Object.GetPaginatedList(pagedQuery);
      Assert.NotNull(pagedList);
      Assert.NotNull(pagedList.Items);
      Assert.True(pagedList.Items.IsNotEmpty());
      Assert.True(pagedList.Items.ToList().Count <= pagedQuery.PageSize);
      Assert.Equal(pagedList.Count, _roles.Count);

      // Act
      _mockRolerService.Verify((s) => s.GetPaginatedList(pagedQuery));
    }

    [Theory]
    [InlineData(_superAdminId)]
    [InlineData(_adminId)]
    public async Task Get_Role_By_Id_Test(string roleId)
    {
      // Arrange
      _mockRolerService.Setup(s => s.GetById(It.IsAny<string>())).ReturnsAsync(() => _roles.First(r => r.ID == roleId));

      // Assert
      var role = await _mockRolerService.Object.GetById(roleId);
      Assert.NotNull(role);
      Assert.True(role.IsEnabled);
      Assert.False(role.IsDeleted);
      Assert.Equal(role.ID, roleId);

      // Act
      _mockRolerService.Verify((s) => s.GetById(roleId));
    }

    [Fact]
    public async Task Get_Role_By_Invalid_Id_Test()
    {
      var roleId = "5eb3f92920d7467aed27c7c6";
      // Arrange
      _mockRolerService.Setup(s => s.GetById(It.IsAny<string>())).ReturnsAsync(() => _roles.FirstOrDefault(r => r.ID == roleId));

      // Assert
      var role = await _mockRolerService.Object.GetById(roleId);
      Assert.Null(role);
      
      // Act
      _mockRolerService.Verify((s) => s.GetById(roleId));
    }
  }
}