using AutoFixture;
using Catalog.Application.Handlers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using NSubstitute;
using NUnit.Framework;

namespace Catalog.Application.Tests.Handlers
{
    [TestFixture]
    public class GetAllProductHandlersTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private GetAllProductHandlers _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetAllProductHandlers(_productRepository);
        }

        [Test]
        public async Task Handle_ReturnsPagedProductResponses()
        {
            var specParams = _fixture.Create<CatalogSpecParams>();
            var products = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .CreateMany(2)
                .ToList();
            var pagination = new Pagination<Product>(1, 10, 2, products);
            _productRepository.GetProductsAsync(specParams).Returns(pagination);

            var result = await _handler.Handle(new GetAllProductsQuery(specParams), CancellationToken.None);

            Assert.That(result.Data, Has.Count.EqualTo(2));
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Handle_EmptyResult_ReturnsEmptyPagination()
        {
            var specParams = _fixture.Create<CatalogSpecParams>();
            var pagination = new Pagination<Product>(1, 10, 0, new List<Product>());
            _productRepository.GetProductsAsync(specParams).Returns(pagination);

            var result = await _handler.Handle(new GetAllProductsQuery(specParams), CancellationToken.None);

            Assert.That(result.Data, Has.Count.EqualTo(0));
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
