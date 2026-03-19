using Catalog.Core.Entities;
using Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class DatabaseSeeder
    {
        // This class will be need to seed the data to the MonggoDB once we start the application and it will configured in the Program.cs

        public DatabaseSeeder()
        {

        }
        public static async Task SeedDataAsync(IOptions<DataBaseSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            var db = client.GetDatabase(options.Value.DatabaseName);
            var brands = db.GetCollection<ProductBrand>(options.Value.BrandCollectionName);
            var types = db.GetCollection<ProductType>(options.Value.TypeCollectionName);
            var products = db.GetCollection<Product>(options.Value.ProductCollectionName);

            // Get the connection string and then put that data to the respecrtve DB

            var SeedBasePath = Path.Combine("Data", "SeedData");

            //Seed Brands
            List<ProductBrand> brandList = new List<ProductBrand>();
            if (await brands.CountDocumentsAsync(_ => true) == 0)
            {
                var brandData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "brand.json"));
                brandList = System.Text.Json.JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                await brands.InsertManyAsync(brandList);
            }
            else
            {
                brandList = await brands.Find(_ => true).ToListAsync();
            }

            // Seed Types
            List<ProductType> typeList = new List<ProductType>();
            if (await types.CountDocumentsAsync(_ => true) == 0)
            {
                var typeData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "type.json"));
                typeList = System.Text.Json.JsonSerializer.Deserialize<List<ProductType>>(typeData);
                await types.InsertManyAsync(typeList);

            }
            else
            {
                typeList = await types.Find(_ => true).ToListAsync();
            }

            // Seed Product
            List<Product> productList = new List<Product>();
            if (await products.CountDocumentsAsync(_ => true) == 0)
            {
                var productData = await File.ReadAllTextAsync(Path.Combine(SeedBasePath, "prodcut.json"));
                productList = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var product in productList)
                {
                    // Reset Id to let Mongo generate one
                    product.Id = null;
                    // Default Created Date if not set
                    if (product.CreatedDate == default)
                    {
                        product.CreatedDate = DateTime.UtcNow;
                    }
                }

                await products.InsertManyAsync(productList);
            }

        }
    }
}