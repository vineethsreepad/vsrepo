using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllProductByNameQuery : IRequest<IEnumerable<ProductResponse>>
    {
        public string Name { get; init; }
    }
}