using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
   public interface IBrandRepository
    {
        /// <summary>
        /// Get all brands
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProductBrand>> GetAllBrandsAsync();

        /// <summary>
        /// Get brand by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductBrand> GetBrandByIdAsync(string id);
    }
}