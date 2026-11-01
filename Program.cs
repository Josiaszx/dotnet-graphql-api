using dotnet_graphql_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddGraphQL().AddTypes();

// Agregar el builder para el DBContextFactory necesario para que una consulta pueda 
// tener varias peticiones en paralelo
builder.Services.AddDbContextFactory<ShopContext>(o => 
        o.UseSqlite("Data Source=shop.db")
    );


var app = builder.Build();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);
