# ATCMediator (Application Task Control Mediator)

**ATCMediator** es una implementación de código abierto del patrón **Mediator** para arquitecturas limpias.  
Funciona como una “torre de control” para tus casos de uso o servicios, centralizando comandos y queries.
La libreria busca fomentar una comunidad para forteceler la experiencia de desarrollo en c# .Net.

## Instalación

```bash
dotnet add package ATCMediator --version 1.0.2

…

## Uso básico

```csharp
// Registrar en DI (en tu Program.cs)
builder.Services.AddATCMediator(
    typeof(Program).Assembly, 
    typeof(GetProductAllQueryHandler).Assembly
);

// Datos Entrada
public class ExampleCommand : ICommand
{
    public string? Name { get; set; }
    public decimal Price { get; set;}
}

// Handler entrada
public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(CreateProductCommand command)
    {
        var product = Domain.Product.Product.Create(command.Name!, command.Price);
        await _productRepository.AddAsync(product);
    }
}

// Datos salida
public class GetProductAllQuery : IQuery<IEnumerable<Domain.Product.Product>> { }

// Handler salida
 public class GetProductAllQueryHandler : IQueryHandler<GetProductAllQuery, IEnumerable<Domain.Product.Product>>
{
    private readonly IProductRepository _productRepository;

    public GetProductAllQueryHandler(
        IProductRepository productRepository
    )
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Domain.Product.Product>> Handle(GetProductAllQuery query)
    {
        var products = await _productRepository.GetAllAsync();
        return products;
    }
}

// Inyectar y usar en tu controlador
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CrearProduct([FromBody] CreateProductCommand createProduct)
    {
        await _mediator.SendCommand(createProduct);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var productos = await _mediator.SendQuery(new GetProductAllQuery());
        return Ok(productos);
    }
}

