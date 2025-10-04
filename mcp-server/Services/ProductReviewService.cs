using System.Reflection;
using System.Text.Json;

namespace RRK.Services;

using RRK.Interfaces;


public class ProductReviewService : IProductReviewService
{
    private readonly List<ProductReview> _productReviews;

    public ProductReviewService()
    {
        _productReviews = GetAllProductReviews();
    }

    public List<ProductReview> GetProductReviews(int productId)
    {
        return _productReviews.Where(r => r.ProductID == productId).ToList();
    }

    public List<ProductReview> GetAllProductReviews()
    {
        var path = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        var reviewsJsonPath = Path.Combine(path, "data", "product-reviews.json");

        if (!File.Exists(reviewsJsonPath))
        {
            throw new FileNotFoundException("product-reviews.json file not found", reviewsJsonPath);
        }

        var jsonContent = File.ReadAllText(reviewsJsonPath);
        var reviews = JsonSerializer.Deserialize<List<ProductReview>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return reviews ?? new List<ProductReview>();
    }
}