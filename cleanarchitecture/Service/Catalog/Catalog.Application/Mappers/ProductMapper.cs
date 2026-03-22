using Catalog.Application.Commands;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers
{
    public static class ProdcutMapper
    {
        public static ProductResponse ToResponse(this Product product)
        {
            if (product == null)
                return null;
            return new ProductResponse
            {
                Id = product.Id,
                Brand = product.Brand,
                CreatedDate = product.CreatedDate,
                Description = product.Description,
                ImageFile = product.Description,
                Name = product.Name,
                Price = product.Price,
                Summary = product.Summary,
                Type = product.Type
            };
        }

        public static IList<ProductResponse> ToResponseList(this IEnumerable<Product> products)
        {
            return products.Select(p => p.ToResponse()).ToList();
        }

        public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination) =>
            new Pagination<ProductResponse>(pagination.PageIndex, pagination.PageSize, pagination.Count, pagination.Data.Select(p => p.ToResponse()).ToList());

        public static Product ToEntity(this CreateProductCommand command, ProductBrand productBrand, ProductType productType)
        {
            return new Product
            {
                Brand = productBrand,
                CreatedDate = DateTimeOffset.UtcNow,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Name = command.Name,
                Price = command.Price,
                Summary = command.Summary,
                Type = productType
            };
        }

        public static Product ToUpdateEntity(this UpdateProductCommand command, Product existingProduct, ProductBrand productBrand, ProductType productType)
        {
            return new Product
            {
                Id = existingProduct.Id,
                Name = command.Name,
                Brand = productBrand,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Type = productType,
                Summary = command.Summary,
                Price = command.Price,
                CreatedDate = existingProduct.CreatedDate

            };
        }

    }
}