using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public record GetAllProductByNameQuery(string Name) : IRequest<IEnumerable<ProductResponse>>
    {
    }
}