# ATCMediator (Application Task Control Mediator)

**ATCMediator** es una implementación de código abierto del patrón **Mediator** para arquitecturas limpias.  
Funciona como una “torre de control” para tus casos de uso o servicios, centralizando comandos y queries.  
La librería busca fomentar una comunidad para fortalecer la experiencia de desarrollo en C# .NET.

---

## Instalación

🚀 Puedes instalar ATCMediator usando NuGet:

```bash
dotnet add package ATCMediator --version 3.0.0
```

## Uso básico

📌 Registrar en DI (en tu Program.cs)

```csharp
builder.Services.AddATCMediator(
    typeof(Program).Assembly, 
    typeof(GetProductAllQueryHandler).Assembly
);
```

✅ Comando de entrada

```csharp
public class CreateProductCommand : ICommand<Guid>
{
    public string? Name { get; set; }
    public decimal Price { get; set;}
}
```

💡 Handler del comando

```csharp
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>  
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = Domain.Product.Product.Create(command.Name!, command.Price);
        await _productRepository.AddAsync(product);
        return product.Id;
    }
}
```

🔍 Consulta de productos
```csharp
public class GetProductAllQuery : IQuery<IEnumerable<Domain.Product.Product>> {}
```

📦 Handler de la consulta
```csharp
public class GetProductAllQueryHandler : IQueryHandler<GetProductAllQuery, IEnumerable<Domain.Product.Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductAllQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Domain.Product.Product>> Handle(GetProductAllQuery query, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync();
        return products;
    }
}
```

🌐 Uso en un controlador ASP.NET Core
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CrearProducto([FromBody] CreateProductCommand createProduct)
    {
        var result = await _mediator.Execute(createProduct);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProductos()
    {
        var productos = await _mediator.Execute(new GetProductAllQuery());
        return Ok(productos);
    }
}
```


