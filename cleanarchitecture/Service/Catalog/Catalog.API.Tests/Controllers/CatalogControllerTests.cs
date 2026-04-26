using AutoFixture;
using Catalog.API.Controllers;
using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;

namespace Catalog.API.Tests.Controllers
{
    [TestFixture]
    public class CatalogControllerTests
    {
        private Fixture _fixture;
        private IMediator _mediator;
        private CatalogContoller _controller;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _mediator = Substitute.For<IMediator>();
            _controller = new CatalogContoller(_mediator);
        }

        [Test]
        public async Task GetAllProducts_ReturnsOkWithPaginatedResult()
        {
            var specParams = _fixture.Create<CatalogSpecParams>();
            var response = new Pagination<ProductResponse>(1, 10, 1, new List<ProductResponse>
            {
                _fixture.Build<ProductResponse>()
                    .With(r => r.Brand, _fixture.Create<ProductBrand>())
                    .With(r => r.Type, _fixture.Create<ProductType>())
                    .Create()
            });
            _mediator.Send(Arg.Any<GetAllProductsQuery>(), Arg.Any<CancellationToken>()).Returns(response);

            var result = await _controller.GetAllProducts(specParams);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetProduct_ReturnsOkWithProduct()
        {
            var id = _fixture.Create<string>();
            var response = _fixture.Build<ProductResponse>()
                .With(r => r.Brand, _fixture.Create<ProductBrand>())
                .With(r => r.Type, _fixture.Create<ProductType>())
                .Create();
            _mediator.Send(Arg.Any<GetProductByIdQuery>(), Arg.Any<CancellationToken>()).Returns(response);

            var result = await _controller.GetProduct(id);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetProductByProductName_ProductsFound_ReturnsOkWithDtoList()
        {
            var name = _fixture.Create<string>();
            var responses = new List<ProductResponse>
            {
                _fixture.Build<ProductResponse>()
                    .With(r => r.Brand, _fixture.Create<ProductBrand>())
                    .With(r => r.Type, _fixture.Create<ProductType>())
                    .Create()
            };
            _mediator.Send(Arg.Any<GetAllProductByNameQuery>(), Arg.Any<CancellationToken>())
                .Returns(responses.AsEnumerable());

            var result = await _controller.GetProductByProductName(name);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetProductByProductName_NullResult_ReturnsNotFound()
        {
            var name = _fixture.Create<string>();
            _mediator.Send(Arg.Any<GetAllProductByNameQuery>(), Arg.Any<CancellationToken>())
                .Returns((IEnumerable<ProductResponse>)null);

            var result = await _controller.GetProductByProductName(name);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetProductByProductName_EmptyList_ReturnsNotFound()
        {
            var name = _fixture.Create<string>();
            _mediator.Send(Arg.Any<GetAllProductByNameQuery>(), Arg.Any<CancellationToken>())
                .Returns(Enumerable.Empty<ProductResponse>());

            var result = await _controller.GetProductByProductName(name);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task CreateProduct_ReturnsOkWithResult()
        {
            var command = _fixture.Create<CreateProductCommand>();
            var response = _fixture.Build<ProductResponse>()
                .With(r => r.Brand, _fixture.Create<ProductBrand>())
                .With(r => r.Type, _fixture.Create<ProductType>())
                .Create();
            _mediator.Send(Arg.Any<CreateProductCommand>(), Arg.Any<CancellationToken>()).Returns(response);

            var result = await _controller.CreateProduct(command);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task DeleteProduct_Success_ReturnsNoContent()
        {
            var id = _fixture.Create<string>();
            _mediator.Send(Arg.Any<DeleteProductByIdCommand>(), Arg.Any<CancellationToken>()).Returns(true);

            var result = await _controller.DeleteProduct(id);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteProduct_NotFound_ReturnsNotFound()
        {
            var id = _fixture.Create<string>();
            _mediator.Send(Arg.Any<DeleteProductByIdCommand>(), Arg.Any<CancellationToken>()).Returns(false);

            var result = await _controller.DeleteProduct(id);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task UpdateProduct_Success_ReturnsNoContent()
        {
            var id = _fixture.Create<string>();
            var dto = _fixture.Build<UpdateProductDto>().With(d => d.Price, 99.99m).Create();
            _mediator.Send(Arg.Any<UpdateProductCommand>(), Arg.Any<CancellationToken>()).Returns(true);

            var result = await _controller.UpdateProduct(id, dto);

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateProduct_NotFound_ReturnsNotFound()
        {
            var id = _fixture.Create<string>();
            var dto = _fixture.Build<UpdateProductDto>().With(d => d.Price, 99.99m).Create();
            _mediator.Send(Arg.Any<UpdateProductCommand>(), Arg.Any<CancellationToken>()).Returns(false);

            var result = await _controller.UpdateProduct(id, dto);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetAllBrands_ReturnsOkWithBrands()
        {
            var brands = _fixture.CreateMany<BrandResponse>(2).ToList();
            _mediator.Send(Arg.Any<GetAllBrandsQuery>(), Arg.Any<CancellationToken>())
                .Returns(brands as IList<BrandResponse>);

            var result = await _controller.GetAllBrands();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetAllTypes_ReturnsOkWithTypes()
        {
            var types = _fixture.CreateMany<TypesResponse>(2).ToList();
            _mediator.Send(Arg.Any<GetAllTypesQuery>(), Arg.Any<CancellationToken>())
                .Returns(types as IList<TypesResponse>);

            var result = await _controller.GetAllTypes();

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public async Task GetProductsByBrand_ReturnsOkWithProducts()
        {
            var brand = _fixture.Create<string>();
            var responses = _fixture.Build<ProductResponse>()
                .With(r => r.Brand, _fixture.Create<ProductBrand>())
                .With(r => r.Type, _fixture.Create<ProductType>())
                .CreateMany(2)
                .ToList();
            _mediator.Send(Arg.Any<GetProdutsByBrandQuery>(), Arg.Any<CancellationToken>())
                .Returns(responses as IList<ProductResponse>);

            var result = await _controller.GetProductsByBrand(brand);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        }
    }
}
