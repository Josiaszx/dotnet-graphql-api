using dotnet_graphql_api.Data;
using dotnet_graphql_api.InputTypes;
using dotnet_graphql_api.Types;
using Microsoft.EntityFrameworkCore;

namespace dotnet;

[MutationType]
public static partial class Mutation
{
    public static async Task<Product> NewProductAsync(
        NewProductInput input, 
        IDbContextFactory<ShopContext> contextFactory)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var product = new Product(input);
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }
}