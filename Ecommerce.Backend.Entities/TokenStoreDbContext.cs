using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Backend.Entities
{
  public class TokenStoreDbContext : DbContext
  {
    public TokenStoreDbContext(DbContextOptions<TokenStoreDbContext> options) : base(options) { }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<CustomerLogin> CustomerLogins { get; set; }
  }
}