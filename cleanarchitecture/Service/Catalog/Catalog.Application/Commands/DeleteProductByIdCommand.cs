using MediatR;

namespace Catalog.Application.Commands
{
    public record DeleteProductByIdCommand(string id) : IRequest<bool>
    {

    }
}