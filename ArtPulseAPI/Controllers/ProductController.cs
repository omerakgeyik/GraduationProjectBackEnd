using Microsoft.AspNetCore.Mvc;
using ArtPulseAPI.DTO;
using ArtPulseAPI.Data;
using ArtPulseAPI.Models;
using Microsoft.EntityFrameworkCore;
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

        //Get all Products
        [HttpGet("allProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _dataContext.Products.ToListAsync();

                if (products.Count == 0)
                {
                    return NotFound("No products found.");
                }

                var productDTOs = products.Select(p => ProductToDTO(p)).ToList();

                return Ok(productDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Get product by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByID(int id)
        {
            try
            {
                var product = await _dataContext.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var productDTO = await ProductToDTO(product);

                return Ok(productDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Add product
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(ProductDTO productDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var seller = await _dataContext.Sellers.FindAsync(productDTO.SellerId);

                if (seller == null)
                {
                    return BadRequest("Seller not found.");
                }

                var product = new Product
                {
                    Amount = productDTO.Amount,
                    RatingScaledBy10 = productDTO.RatingScaledBy10,
                    Category = (Category)Enum.Parse(typeof(Category), productDTO.Category),
                    Cost = productDTO.Cost,
                    Name = productDTO.Name,
                    Details = productDTO.Details,
                    Seller = seller
                };

                _dataContext.Products.Add(product);
                await _dataContext.SaveChangesAsync();

                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Update product
        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            try
            {
                if (id != productDTO.Id)
                {
                    return BadRequest("Product ID mismatch.");
                }

                var product = await _dataContext.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                var seller = await _dataContext.Sellers.FindAsync(productDTO.SellerId);

                if (seller == null)
                {
                    return BadRequest("Seller not found.");
                }

                product.Amount = productDTO.Amount;
                product.RatingScaledBy10 = productDTO.RatingScaledBy10;
                product.Category = (Category)Enum.Parse(typeof(Category), productDTO.Category);
                product.Cost = productDTO.Cost;
                product.Name = productDTO.Name;
                product.Details = productDTO.Details;
                product.Seller = seller;

                await _dataContext.SaveChangesAsync();

                return Ok("Product updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Delete product
        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _dataContext.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound("Product not found.");
                }

                _dataContext.Products.Remove(product);
                await _dataContext.SaveChangesAsync();

                return Ok("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Get Products by Ascending order
        [HttpGet("ProductsPriceAsc")]
        public async Task<IActionResult> GetProductsPriceAsc()
        {
            try
            {
                var products = await _dataContext.Products
                    .OrderBy(p => p.Cost)
                    .ToListAsync();

                if (products.Count == 0)
                {
                    return NotFound("No products found.");
                }

                var productDTOs = products.Select(p => ProductToDTO(p)).ToList();

                return Ok(productDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Get Products by Descanding order
        [HttpGet("ProductsPriceDesc")]
        public async Task<IActionResult> GetProductsPriceDesc()
        {
            try
            {
                var products = await _dataContext.Products
                    .OrderByDescending(p => p.Cost)
                    .ToListAsync();

                if (products.Count == 0)
                {
                    return NotFound("No products found.");
                }

                var productDTOs = products.Select(p => ProductToDTO(p)).ToList();

                return Ok(productDTOs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Get top 10 products
        [HttpGet("BestProducts")]
        public async Task<IActionResult> GetBestProducts()
        {
            try
            {
                var bestProducts = await _dataContext.Products
                    .OrderByDescending(p => p.RatingScaledBy10)
                    .Take(10)
                    .Select(p => ProductToDTO(p))
                    .ToListAsync();

                if (bestProducts.Count == 0)
                {
                    return NotFound("No products found.");
                }

                return Ok(bestProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<IActionResult> ProductToDTO(Product product)
        {
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Amount = product.Amount,
                RatingScaledBy10 = product.RatingScaledBy10,
                Category = Enum.GetName(typeof(Category), product.Category), // Convert enum to string
                Cost = product.Cost,
                Name = product.Name,
                Details = product.Details,
                SellerId = product.Seller.Id  // Assuming Seller has an Id property
            };

            return Ok(productDTO);
        }
    }
}
