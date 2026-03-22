using System;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public record GetAllBrandsQuery : IRequest<IList<BrandResponse>>;

    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponse>>
    {
        private readonly IBrandRepository _brandRespository;
        public GetAllBrandsHandler(IBrandRepository brandRepository)
        {
            _brandRespository = brandRepository;
        }
        public async Task<IList<BrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brandList = await _brandRespository.GetAllBrandsAsync();
            return brandList.ToResponseList();
        }
    }
}
