using dotnet_graphql_api.InputTypes;

namespace dotnet_graphql_api.Types;

public record Product(int Id, string Name, int Price, int Stock)
{
    public Product(NewProductInput newProduct) : 
        this(0, newProduct.Name, newProduct.Price, newProduct.Stock) 
    {}
}