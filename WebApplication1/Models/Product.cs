using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;
public class Product
{
    public int ProductId { get; set; } 
    public String Name { get; set; }
    public int CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
    public int ProductPriceId { get; set; }
    [ForeignKey(nameof(ProductPriceId))]
    public ProductPrice ProductPrice { get; set; }
}