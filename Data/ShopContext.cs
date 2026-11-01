using dotnet_graphql_api.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace dotnet_graphql_api.Data;

public class ShopContext(DbContextOptions<ShopContext> options) : DbContext(options) 
{
    public DbSet<Sale>  Sales { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Branch> Branches { get; set; }
}