using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetProductByBrandHandler : IRequestHandler<GetProdutsByBrandQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductByBrandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IList<ProductResponse>> Handle(GetProdutsByBrandQuery request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetProductsByBrandAsync(request.BrandName);
            return result.ToResponseList();
        }
    }
}