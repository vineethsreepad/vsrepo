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
    public class GetProductByBrandHandlerTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private GetProductByBrandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetProductByBrandHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ProductsFound_ReturnsMappedList()
        {
            var brandName = _fixture.Create<string>();
            var products = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .CreateMany(2)
                .ToList();
            _productRepository.GetProductsByBrandAsync(brandName).Returns(products);

            var result = await _handler.Handle(new GetProdutsByBrandQuery(brandName), CancellationToken.None);

            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task Handle_NoProducts_ReturnsEmptyList()
        {
            var brandName = _fixture.Create<string>();
            _productRepository.GetProductsByBrandAsync(brandName).Returns(Enumerable.Empty<Product>());

            var result = await _handler.Handle(new GetProdutsByBrandQuery(brandName), CancellationToken.None);

            Assert.That(result, Is.Empty);
        }
    }
}
