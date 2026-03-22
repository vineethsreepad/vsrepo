using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetProdutsByBrandQuery() : IRequest<IList<ProductResponse>>
    {
        public string BrandName { get; set; }
    }
}
