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
    public class CreateProductCommandHandlerTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private CreateProductCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new CreateProductCommandHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsProductResponse()
        {
            var command = _fixture.Create<CreateProductCommand>();
            var brand = _fixture.Create<ProductBrand>();
            var type = _fixture.Create<ProductType>();
            var product = _fixture.Build<Product>()
                .With(p => p.Brand, brand)
                .With(p => p.Type, type)
                .Create();

            _productRepository.GetBrandByIdAsync(command.BrandId).Returns(brand);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns(type);
            _productRepository.CreateProductAsync(Arg.Any<Product>()).Returns(product);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
        }

        [Test]
        public void Handle_BrandNotFound_ThrowsApplicationException()
        {
            var command = _fixture.Create<CreateProductCommand>();
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns((ProductBrand)null);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns(_fixture.Create<ProductType>());

            Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_TypeNotFound_ThrowsApplicationException()
        {
            var command = _fixture.Create<CreateProductCommand>();
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns(_fixture.Create<ProductBrand>());
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns((ProductType)null);

            Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public void Handle_BothBrandAndTypeNull_ThrowsApplicationException()
        {
            var command = _fixture.Create<CreateProductCommand>();
            _productRepository.GetBrandByIdAsync(command.BrandId).Returns((ProductBrand)null);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns((ProductType)null);

            Assert.ThrowsAsync<ApplicationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Test]
        public async Task Handle_ValidCommand_CallsCreateProductAsyncOnce()
        {
            var command = _fixture.Create<CreateProductCommand>();
            var brand = _fixture.Create<ProductBrand>();
            var type = _fixture.Create<ProductType>();
            var product = _fixture.Build<Product>()
                .With(p => p.Brand, brand)
                .With(p => p.Type, type)
                .Create();

            _productRepository.GetBrandByIdAsync(command.BrandId).Returns(brand);
            _productRepository.GetTypesByIdAsync(command.TypeId).Returns(type);
            _productRepository.CreateProductAsync(Arg.Any<Product>()).Returns(product);

            await _handler.Handle(command, CancellationToken.None);

            await _productRepository.Received(1).CreateProductAsync(Arg.Any<Product>());
        }
    }
}
