using AutoFixture;
using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Catalog.Application.Tests.Handlers
{
    [TestFixture]
    public class DeleteProductByIdCommandHandlerTests
    {
        private Fixture _fixture;
        private IProductRepository _productRepository;
        private DeleteProductByIdCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _productRepository = Substitute.For<IProductRepository>();
            _handler = new DeleteProductByIdCommandHandler(_productRepository);
        }

        [Test]
        public async Task Handle_ProductExists_ReturnsTrue()
        {
            var id = _fixture.Create<string>();
            var command = new DeleteProductByIdCommand(id);
            _productRepository.DeleteProductAsync(id).Returns(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task Handle_ProductNotFound_ReturnsFalse()
        {
            var id = _fixture.Create<string>();
            var command = new DeleteProductByIdCommand(id);
            _productRepository.DeleteProductAsync(id).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result, Is.False);
        }
    }
}
