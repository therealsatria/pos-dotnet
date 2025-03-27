using Infrastructures.Models;
using Infrastructures.Repositories;
using Infrastructures.Exceptions;
using Infrastructures.DTOs;
using System;
using System.Threading.Tasks;

namespace Infrastructures.Services
{
    public class ProductService : GenericService<Product>
    {
        public ProductService(IGenericRepository<Product> repository) 
            : base(repository)
        {
        }

        /// <summary>
        /// Creates a new product from the provided DTO
        /// </summary>
        /// <param name="dto">Product creation data</param>
        /// <returns>Created product</returns>
        public async Task<Product> CreateProductAsync(ProductCreateRequestDto dto)
        {
            // Map DTO to Product entity
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Category = dto.Category,
                Barcode = dto.Barcode
            };

            // Add product to repository
            await AddAsync(product);
            return product;
        }

        /// <summary>
        /// Updates an existing product with data from the provided DTO
        /// </summary>
        /// <param name="dto">Product update data</param>
        /// <returns>Updated product</returns>
        public async Task<Product> UpdateProductAsync(ProductUpdateRequestDto dto)
        {
            // Get existing product
            var product = await GetByIdAsync(dto.Id);

            // Update only provided fields
            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.Category != null) product.Category = dto.Category;
            if (dto.Barcode != null) product.Barcode = dto.Barcode;

            // Update product in repository
            await UpdateAsync(product);
            return product;
        }

        /// <summary>
        /// Adjusts product stock quantity by specified amount
        /// </summary>
        /// <param name="productId">Product ID</param>
        /// <param name="quantityChange">Amount to adjust stock by (positive or negative)</param>
        /// <returns>Updated product</returns>
        public async Task<Product> AdjustStockAsync(Guid productId, int quantityChange)
        {
            // Get existing product
            var product = await GetByIdAsync(productId);

            // Calculate new stock quantity
            var newQuantity = product.StockQuantity + quantityChange;
            if (newQuantity < 0)
            {
                throw new ValidationException("Stock quantity cannot be negative");
            }

            // Update stock quantity
            product.StockQuantity = newQuantity;
            await UpdateAsync(product);
            return product;
        }
    }
}
