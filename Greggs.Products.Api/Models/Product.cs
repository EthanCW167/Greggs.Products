namespace Greggs.Products.Api.Models;

public class Product
{
    public string Name { get; set; }
    public decimal PriceInPounds { get; set; }

    public Product Clone() // Create clone of Product Object with new reference
    {
        return new Product {Name = this.Name, PriceInPounds = this.PriceInPounds};
    }
}