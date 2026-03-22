using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {

        private readonly IProductRepository _productRepository;
        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // fetch brand and type from repository
            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);

            // fetch type
            var type = await _productRepository.GetTypesByIdAsync(request.TypeId);

            if (brand == null || type == null)
                throw new ApplicationException("Invalid brand or type specified");


            var result = await _productRepository.CreateProductAsync(request.ToEntity(brand, type));
            return result.ToResponse();
        }
    }
}