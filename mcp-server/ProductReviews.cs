

using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;
using RRK.Services;
using static ToolsInformation;


public class ProductReviews
{
    private readonly ILogger<ProductReviews> _logger;
    private readonly IProductReviewService _productReviewService;

    public ProductReviews(ILogger<ProductReviews> logger, IProductReviewService productReviewService)
    {
        _logger = logger;
        _productReviewService = productReviewService;
    }

    [Function(nameof(GetProductReviews))]
    public string GetProductReviews([McpToolTrigger(GetProductReviewsToolName, GetProductReviewsToolDescription)] ProductReviewRequest request,
            ToolInvocationContext context
            )
    {

        int productId = request.ProductId;

        _logger.LogInformation("GetProductReviews function received a request for Product ID: {ProductId}.", productId);

        var filteredReviews = _productReviewService.GetProductReviews(productId);
        var jsonResponse = JsonSerializer.Serialize(filteredReviews);

        return jsonResponse;
    }


}