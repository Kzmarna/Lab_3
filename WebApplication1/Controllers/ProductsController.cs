using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsDbContext _context;

    public ProductsController(ProductsDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _context.Product.Include(e => e.Category).Include(s => s.ProductPrice).ToListAsync();
        return Ok(products);
    }

    [HttpGet]
    [Route("get-product-by-id")]
    public async Task<IActionResult> GetOneProduct(int productId)
    {
        var product = await _context.Product.Include(e => e.Category).Include(s => s.ProductPrice)
            .FirstOrDefaultAsync(i => i.ProductId == productId);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(Product newProduct)
    {
        newProduct.ProductPrice = new ProductPrice()
        {
            ProductPriceId = newProduct.ProductPriceId,
            Value = newProduct.ProductPrice.Value,
            Currency = newProduct.ProductPrice.Currency
        };
        var category = await _context.Category.FirstOrDefaultAsync(i => i.CategoryId == newProduct.CategoryId);
        if (category == null)
        {
            return NotFound();
        }
        newProduct.Category = category;
        _context.Product.Add(newProduct);
        await _context.SaveChangesAsync();
        return Created($"/get-product-by-id?productId={newProduct.ProductId}", newProduct);
    }

    [HttpDelete]
    [Route("{productId}")]
    public async Task<IActionResult> DeleteProduct(int productId)
    {
        var product = await _context.Product.FindAsync(productId);
        if (product == null)
        {
            return NotFound();
        }

        _context.Product.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProduct(int productId, Product productUpdate)
    {
        var product = await _context.Product.FirstOrDefaultAsync(i => i.ProductId == productId);
        if (product == null)
        {
            return NotFound();
        }
        product.ProductPriceId = productUpdate.ProductPriceId;
        product.ProductPrice = new ProductPrice()
        {
            ProductPriceId = productUpdate.ProductPriceId,
            Value = productUpdate.ProductPrice.Value,
            Currency = productUpdate.ProductPrice.Currency
        };
        await _context.ProductPrice.AddAsync(product.ProductPrice);
        product.Name = productUpdate.Name;
        product.CategoryId = productUpdate.CategoryId;
        var category = await _context.Category.FirstOrDefaultAsync(i => i.CategoryId == productUpdate.CategoryId);
        if (category == null)
        {
            return NotFound();
        }
        product.Category = category;
        _context.Product.Update(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}