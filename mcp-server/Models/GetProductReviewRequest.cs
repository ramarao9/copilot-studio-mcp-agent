

using System.ComponentModel;

public class ProductReviewRequest
{

    [Description("The ID of the product to get reviews for.")]
    public required int ProductId { get; set; }
}