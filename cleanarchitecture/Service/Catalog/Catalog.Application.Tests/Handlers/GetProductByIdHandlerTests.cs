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
    public class GetProductByIdHandlerTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private GetProductByIdHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new GetProductByIdHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ProductExists_ReturnsMappedResponse()
        {
            var id = _fixture.Create<string>();
            var product = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();
            _productRepository.GetProductAsync(id).Returns(product);

            var result = await _handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Price, Is.EqualTo(product.Price));
        }

        [Test]
        public async Task Handle_ProductNull_ReturnsNull()
        {
            var id = _fixture.Create<string>();
            _productRepository.GetProductAsync(id).Returns((Product)null);

            var result = await _handler.Handle(new GetProductByIdQuery(id), CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
