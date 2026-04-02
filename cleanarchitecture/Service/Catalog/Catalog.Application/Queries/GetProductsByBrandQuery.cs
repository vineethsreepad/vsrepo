using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Queries
{
    public record GetProdutsByBrandQuery(string BrandName) : IRequest<IList<ProductResponse>>
    {

    }
}
