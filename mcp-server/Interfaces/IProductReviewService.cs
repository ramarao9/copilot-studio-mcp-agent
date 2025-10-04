

namespace RRK.Interfaces;
public interface IProductReviewService
{
    List<ProductReview> GetProductReviews(int productId);
    List<ProductReview> GetAllProductReviews();
}