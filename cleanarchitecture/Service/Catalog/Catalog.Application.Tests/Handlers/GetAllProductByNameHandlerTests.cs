using AutoFixture;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Catalog.Application.Tests.Handlers
{
    [TestFixture]
    public class GetAllProductByNameHandlerTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private GetAllProductByNameHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetAllProductByNameHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ProductsFound_ReturnsMappedList()
        {
            var name = _fixture.Create<string>();
            var products = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .CreateMany(3)
                .ToList();
            _productRepository.GetProductsByNameAsync(name).Returns(products);

            var result = await _handler.Handle(new GetAllProductByNameQuery(name), CancellationToken.None);

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task Handle_NoProducts_ReturnsEmptyList()
        {
            var name = _fixture.Create<string>();
            _productRepository.GetProductsByNameAsync(name).Returns(Enumerable.Empty<Product>());

            var result = await _handler.Handle(new GetAllProductByNameQuery(name), CancellationToken.None);

            Assert.That(result, Is.Empty);
        }
    }
}
