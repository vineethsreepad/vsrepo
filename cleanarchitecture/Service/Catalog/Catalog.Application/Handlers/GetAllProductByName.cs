using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllProductByNameHandler : IRequestHandler<GetAllProductByNameQuery, IEnumerable<ProductResponse>>
    {

        private readonly IProductRepository _productRepository;
        public GetAllProductByNameHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductByNameQuery request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetProductsByNameAsync(request.Name);
            return result.ToResponseList();
        }
    }
}