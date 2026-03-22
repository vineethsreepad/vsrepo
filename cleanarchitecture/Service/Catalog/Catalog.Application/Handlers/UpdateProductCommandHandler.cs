using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetProductAsync(request.Id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Product wwith Id {request.Id} not found");
            }

            // fetch brand and type
            var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);

            // fetch type
            var type = await _productRepository.GetTypesByIdAsync(request.TypeId);

            if (brand == null || type == null)
                throw new ApplicationException("Invalid brand or type specified");

            // Step 2: Mapper And Update
            var result = await _productRepository.UpdateProductAsync(request.ToUpdateEntity(existingProduct, brand, type));


            // Step 3: Return the bool if Successful
            return result;

        }
    }
}