using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
namespace WebApplication1.DataAccess;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options): base(options) {

    }
    public DbSet<Product> Product => Set<Product>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<ProductPrice> ProductPrice => Set<ProductPrice>();
}