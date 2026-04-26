using AutoFixture;
using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Catalog.Application.Tests.Handlers
{
    [TestFixture]
    public class UpdateProductCommandHandlerTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private UpdateProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ProductExistsAndValidBrandType_ReturnsTrue()
        {
            var command = _fixture.Create<UpdateProductCommand>();
            var existingProduct = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();
            var brand = _fixture.Create<ProductBrand>();
            var type = _fixture.Create<ProductType>();

            _productRepository.GetProductAsync(command.Id).Returns(existingProduct);
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns(brand);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns(type);
            _productRepository.UpdateProductAsync(Arg.Any<Product>()).Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Handle_ProductNotFound_ThrowsKeyNotFoundException()
        {
            var command = _fixture.Create<UpdateProductCommand>();
            _productRepository.GetProductAsync(command.Id).Returns((Product)null);

            Assert.ThrowsAsync<KeyNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_BrandNotFound_ThrowsApplicationException()
        {
            var command = _fixture.Create<UpdateProductCommand>();
            var existingProduct = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();

            _productRepository.GetProductAsync(command.Id).Returns(existingProduct);
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns((ProductBrand)null);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns(_fixture.Create<ProductType>());

            Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_TypeNotFound_ThrowsApplicationException()
        {
            var command = _fixture.Create<UpdateProductCommand>();
            var existingProduct = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();

            _productRepository.GetProductAsync(command.Id).Returns(existingProduct);
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns(_fixture.Create<ProductBrand>());
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns((ProductType)null);

            Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ValidCommand_CallsUpdateProductWithPreservedIdAndCreatedDate()
        {
            var command = _fixture.Create<UpdateProductCommand>();
            var existingProduct = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();
            var brand = _fixture.Create<ProductBrand>();
            var type = _fixture.Create<ProductType>();

            _productRepository.GetProductAsync(command.Id).Returns(existingProduct);
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns(brand);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns(type);
            _productRepository.UpdateProductAsync(Arg.Any<Product>()).Returns(true);

            await _handler.Handle(command, CancellationToken.None);

            await _productRepository.Received(1).UpdateProductAsync(
                Arg.Is<Product>(p =>
                    p.Id == existingProduct.Id &&
                    p.CreatedDate == existingProduct.CreatedDate));
        }
    }
}
