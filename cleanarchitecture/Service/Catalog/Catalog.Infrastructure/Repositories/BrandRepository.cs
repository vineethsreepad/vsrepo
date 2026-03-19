using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;

        public BrandRepository(IOptions<DataBaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.DatabaseName);
            _brands = db.GetCollection<ProductBrand>(options.Value.BrandCollectionName);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ProductBrand>> GetAllBrandsAsync()
        {
            return await _brands.Find(_ => true).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<ProductBrand> GetBrandByIdAsync(string id)
        {
            return await _brands.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}