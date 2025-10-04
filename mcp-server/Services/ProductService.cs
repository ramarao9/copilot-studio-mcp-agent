

namespace RRK.Services;
using System.Reflection;
using System.Text.Json;
using RRK.Interfaces;

public class ProductService : IProductService
{
    public List<Product> GetAllProducts()
    {
        var path = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        var productsJsonPath = Path.Combine(path, "data", "products.json");

        if (!File.Exists(productsJsonPath))
        {
            throw new FileNotFoundException("products.json file not found", productsJsonPath);
        }

        var jsonContent = File.ReadAllText(productsJsonPath);
        var products = JsonSerializer.Deserialize<List<Product>>(jsonContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return products ?? new List<Product>();
    }
}