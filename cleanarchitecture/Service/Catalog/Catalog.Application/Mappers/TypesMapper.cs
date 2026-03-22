using System.Globalization;
using Catalog.Application.Responses;
using Catalog.Core.Entities;

namespace Catalog.Application.Mappers
{
    public static class TypesMapper
    {
        public static TypesResponse ToResponse(this ProductType productType)
        {
            return new TypesResponse
            {
                Id = productType.Id,
                Name = productType.Name
            };
        }

        public static IList<TypesResponse> ToResponseList(this IEnumerable<ProductType> productTypes)
        {
            return productTypes.Select(types => types.ToResponse()).ToList();
        }
    }
}