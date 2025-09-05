
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using RRK.Services;

namespace RRK.ListProducts;
using static ToolsInformation;

public class ListProducts
{
    private readonly ILogger<ListProducts> _logger;
    private readonly IProductService _productService;

    public ListProducts(ILogger<ListProducts> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [Function(nameof(ListAllProducts))]
    public string ListAllProducts([McpToolTrigger(ListProductsToolName, ListProductsToolDescription)]
            ToolInvocationContext context
            )
    {
        _logger.LogInformation("ListProducts function processed a request.");

        var products = _productService.GetAllProducts();
        var jsonResponse = JsonSerializer.Serialize(products, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return jsonResponse;
    }


}