using Microsoft.AspNetCore.Mvc;
using ArtPulseAPI.DTO;
using ArtPulseAPI.Data;
using ArtPulseAPI.Models;
namespace ArtPulseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        readonly DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("/priceAsc/{takeCount}")]
        public IActionResult GetProductsPriceAsc(int takeCount)
        {
            Product[] retreivedProducts = _dataContext.Products.OrderBy(p => p.Cost).Take(takeCount).ToArray();
            return Ok(ProductToProductDTO(retreivedProducts));
        }
        [HttpGet("/priceDesc/{takeCount}")]
        public IActionResult GetProductsPriceDesc(int takeCount)
        {
            Product[] retreivedProducts = _dataContext.Products.OrderByDescending(p => p.Cost).Take(takeCount).ToArray();
            return Ok(ProductToProductDTO(retreivedProducts));
        }

        [HttpGet("/{takeCount}/{category}")]
        public IActionResult GetBestProducts(int takeCount, Category category)
        {
            Product[] retreivedProducts = _dataContext.Products.Where(p=> p.Category == category).OrderByDescending(p => p.RatingScaledBy10).Take(takeCount).ToArray();
            return Ok(ProductToProductDTO(retreivedProducts));
        }
        [HttpGet("/{takeCount}")]
        public IActionResult GetBestProducts(int takeCount)
        {
            Product[] retreivedProducts = _dataContext.Products.OrderByDescending(p => p.RatingScaledBy10).Take(takeCount).ToArray();
            if(retreivedProducts.Length == 0)
            {
                return NotFound();
            }
            return Ok(ProductToProductDTO(retreivedProducts));
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            Product[] retreivedProducts = _dataContext.Products.ToArray();
            return Ok(ProductToProductDTO(retreivedProducts));
        }


        ProductDTO[] ProductToProductDTO(Product[] products)
        {
            ProductDTO[] productDTOArray = new ProductDTO[products.Length];
            for (int i = 0; i < products.Length; i++)
            {
                Product product = products[i];
                productDTOArray[i] = new ProductDTO()
                {
                    SellerId = product.Seller.Id,
                    Amount = product.Amount,
                    Category = "placeholder category",
                    Cost = product.Cost,
                    Details = product.Details,
                    Id = product.Id,
                    Name = product.Name,
                    RatingScaledBy10 = product.RatingScaledBy10
                };
            }
            return productDTOArray;
        }
    }
}
