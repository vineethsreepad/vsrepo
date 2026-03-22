using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypesResponse>>
    {
        private readonly ITypeRepository _typesRepository;
        public GetAllTypesHandler(ITypeRepository typesRespository)
        {
            _typesRepository = typesRespository;
        }
        public async Task<IList<TypesResponse>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var result = await _typesRepository.GetAllTypesAsync();
            return result.ToResponseList();
        }
    }
}