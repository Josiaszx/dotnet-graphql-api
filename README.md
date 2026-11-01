# Implementación API GraphQL .NET

1. Instalar HotChocolate:

```bash
dot net dotnet new install HotChocolate.Templates
```

2. Iniciar proyecto con plantilla 

```bash
dotnet new graphql --name GettingStarted
```
O si queremos agregarlo a un proyecto ya existente:

```bash
dotnet add package HotChocolate.AspNetCore
```

3. Definimos modelos de dominio y DbContext. No se se deben de cargar con EF Core.

```csharp
public class Author
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public int AuthorId { get; set; }
}

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
}
```

4. Configurar el `Program.cs` para agregar el DbContextBuilder.

```csharp
builder.Services.AddDbContext<LibraryContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder
    .AddGraphQL()
    .AddTypes(); // método generado: registra todos los [QueryType], [MutationType], etc.

var app = builder.Build();

app.MapGraphQL(); // expone /graphql
```

El DbContext los configuramos así porque las querys pueden hacer consultas paralelelas con varios hilos.

5. Definimos un archivo `Query` donde definimos los resolvers para las querys.

```csharp
[QueryType]
public static partial class Query
{
    public static async Task<List<Book>> GetBooksAsync(LibraryContext db, CancellationToken ct)
        => await db.Books.AsNoTracking().ToListAsync(ct);

    public static async Task<Book?> GetBookByIdAsync(int id, LibraryContext db, CancellationToken ct)
        => await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id, ct);
}
```

Con este archivo definimos los resolvers y con los tipos de datos que se retornan definimos los objetos del esquema.

6. Definimos un archivo `Resolver` para configurar los resolvers para las peticiones de creacion de recursos.

```csharp
[MutationType]
public static partial class Mutation
{
    public static async Task<Book> AddBookAsync(
        string title, int authorId, LibraryContext db, CancellationToken ct)
    {
        var book = new Book { Title = title, AuthorId = authorId };
        db.Books.Add(book);
        await db.SaveChangesAsync(ct);
        return book;
    }
}
```