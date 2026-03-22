using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Application.Handlers
{
    public record GetAllProductsQuery(CatalogSpecParams CatalogSpecParams) : IRequest<Pagination<ProductResponse>>
    {
    }
    public class GetAllProductHandlers : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
    {

        private readonly IProductRepository _productRepository;
        public GetAllProductHandlers(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetProductsAsync(request.CatalogSpecParams);
            return result.ToResponse();
        }
    }
}