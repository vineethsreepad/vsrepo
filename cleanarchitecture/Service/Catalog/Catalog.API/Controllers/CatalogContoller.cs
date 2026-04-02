using Catalog.Application.DTOs;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Mappers;
using Catalog.Application.Commands;
using Catalog.Application.Responses;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogContoller : ControllerBase
    {

        private readonly IMediator _mediator;
        public CatalogContoller(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IList<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams specParams)
        {
            var query = new GetAllProductsQuery(specParams);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // [HttpGet("{brand/{branName}}")]
        // public async Task<ActionResult<IList<ProductDto>>> GetProductsByBrand(string brandName)
        // {
        //     var query = new GetProdutsByBrandQuery(brandName);
        //     var result = await _mediator.Send(query);
        //     return Ok(result);
        // }

        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductByProductName(string productName)
        {
            var query = new GetAllProductByNameQuery(productName);
            var result = await _mediator.Send(query);
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            return Ok(result.Select(p => p.ToDto()).ToList());
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductByIdCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductDto updateProductDto)
        {
            var command = updateProductDto.ToCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("/brand/{brand}", Name = "GetProductsByBrandName")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(string brand)
        {
            // First get products
            var query = new GetProdutsByBrandQuery(brand);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}