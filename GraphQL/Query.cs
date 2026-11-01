using dotnet_graphql_api.Data;
using dotnet_graphql_api.Types;
using Microsoft.EntityFrameworkCore;

namespace dotnet;

[QueryType]
public static partial class Query
{
    // query resolvers
    public static async Task<IEnumerable<Product>>  GetProductsAsync(
        IDbContextFactory<ShopContext> factory
    )
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Products.ToListAsync();
    }

    public static async Task<IEnumerable<Branch>> GetBranchesAsync(
        IDbContextFactory<ShopContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Branches.ToListAsync();
    }

    public static async Task<IEnumerable<Sale>> GetSalesAsync(
        IDbContextFactory<ShopContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();
        return await db.Sales.ToListAsync();
    }

    public static async Task<IEnumerable<Sale>> GetSaleByFilterAsync(
        int? id,
        int? productId,
        int? branchId,
        IDbContextFactory<ShopContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();
        IQueryable<Sale> sales = db.Sales;

        if (id is not null) 
            sales = sales.Where(s => s.Id == id.Value);

        if (productId is not null)
            sales = sales.Where(s => s.ProductId == productId.Value);

        if (branchId is not null)
            sales = sales.Where(s => s.BranchId == branchId.Value);
        
        return await sales.AsNoTracking().ToListAsync();
    }
}
