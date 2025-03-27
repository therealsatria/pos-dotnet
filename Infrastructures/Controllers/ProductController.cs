using Microsoft.AspNetCore.Mvc;
using Infrastructures.Services;
using Infrastructures.DTOs;
using Infrastructures.Models;
using System;
using System.Threading.Tasks;

namespace Infrastructures.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get a specific product by ID
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>Product details</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="dto">Product creation data</param>
        /// <returns>Created product</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequestDto dto)
        {
            var product = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="dto">Product update data</param>
        /// <returns>Updated product</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequestDto dto)
        {
            var product = await _productService.UpdateProductAsync(dto);
            return Ok(product);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Adjust product stock quantity
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <param name="quantityChange">Amount to adjust stock by</param>
        /// <returns>Updated product</returns>
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> AdjustStock(Guid id, [FromQuery] int quantityChange)
        {
            var product = await _productService.AdjustStockAsync(id, quantityChange);
            return Ok(product);
        }
    }
}
