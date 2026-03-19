using System.Net;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly IMongoCollection<ProductBrand> _brands;
        private readonly IMongoCollection<ProductType> _types;
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["DatabaseSettings:ConnectionString"]);
            var db = client.GetDatabase(configuration["DatabaseSettings:DatabaseName"]);
            _brands = db.GetCollection<ProductBrand>(configuration["DatabaseSettings:BrandCollectionName"]);
            _types = db.GetCollection<ProductType>(configuration["DatabaseSettings:TypeCollectionName"]);
            _products = db.GetCollection<Product>(configuration["DatabaseSettings:ProductCollectionName"]);
        }   
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(string productId)
        {
           var deleteProduct = await _products.DeleteOneAsync(p => p.Id == productId);
           return deleteProduct.IsAcknowledged && deleteProduct.DeletedCount > 0;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string brandId)
        {
            return await _brands.Find(b => b.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            return await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        
        // This is most important method wehre we are implementing the Pagination
        public async Task<Pagination<Product>> GetProductsAsync(CatalogSpecParams catalogSpecParams)
        {
            var builder = Builders<Product>.Filter; // First add the product filter
            var filter = builder.Empty;

            // This is for the Seach Spec - Search Criteria
            if(!string.IsNullOrEmpty(catalogSpecParams.Search))
            {
                filter = builder.Where(p => p.Name.ToLower().Contains(catalogSpecParams.Search.ToLower()));
            }

            // This is for the Brand Spec -> this will return for BrandId - This is for the Filter Criteria
            if(!string.IsNullOrEmpty(catalogSpecParams.BrandId))
            {
                filter &= builder.Eq(p => p.Brand.Id, catalogSpecParams.BrandId);
            }

            // This is for the Type Spec -> this will return for TypeId - This for the Filter Criteria
            if(!string.IsNullOrEmpty(catalogSpecParams.TypeId))
            {
                filter &= builder.Eq(p => p.Type.Id, catalogSpecParams.TypeId);
            }

            // This will give us the toatl count of the products
            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(catalogSpecParams, filter);

            return new Pagination<Product>(catalogSpecParams.PageIndex, catalogSpecParams.PageIndex, (int)totalItems, data);
        }

        public async Task<IEnumerable<Product>> GetProductsByBrandAsync(string name)
        {
            return await _products
            .Find(p => p.Brand.Name.ToLower() == name.ToLower())
            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            // Important: We have used this builder expression where we are finding the sub-string search // Shoes or Shoe -> It can b any shoe Example: Running Shoe, Walking Shoe, or Nike Shoe or Addidas Shoe
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression($".*{name}.*", "i")); 
            return await _products.Find(filter).ToListAsync();
        }

        public async Task<ProductType> GetTypesByIdAsync(string typeId)
        {
            return await _types.Find(t => t.Id == typeId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updatedProduct = await _products
            .ReplaceOneAsync(p => p.Id == product.Id, product); // Note how this is done in Reports Page for Reports Application
            return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
        }

        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
        {
            var sortDefition = Builders<Product>.Sort.Ascending(p => p.Name);
            if(!string.IsNullOrEmpty(catalogSpecParams.SortOption))
            {
                sortDefition = catalogSpecParams.SortOption switch
                {
                    "priceAsc" => Builders<Product>.Sort.Ascending( p => p.Price),
                    "priceDesc" => Builders<Product>.Sort.Descending(p => p.Price),
                    _ => Builders<Product>.Sort.Ascending(p => p.Name)
                };
            }
            
            return await _products
            .Find(filter)
            .Sort(sortDefition)
            .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex -1)) // This is for the Pagination - Skip the number of items based on the page size and page index
            .Limit(catalogSpecParams.PageSize) // This is for the Pagination - Limit the
            .ToListAsync();
        }
    }
}