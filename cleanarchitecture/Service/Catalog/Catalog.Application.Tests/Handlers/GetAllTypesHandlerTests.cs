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
    public class GetAllTypesHandlerTests
    {
        private Fixture _fixture;
        private ITypeRepository _typeRepository;
        private GetAllTypesHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _typeRepository = Substitute.For<ITypeRepository>();
            _handler = new GetAllTypesHandler(_typeRepository);
        }

        [Test]
        public async Task Handle_TypesExist_ReturnsMappedTypesResponses()
        {
            var types = _fixture.CreateMany<ProductType>(3).ToList();
            _typeRepository.GetAllTypesAsync().Returns(types);

            var result = await _handler.Handle(new GetAllTypesQuery(), CancellationToken.None);

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo(types[0].Id));
            Assert.That(result[0].Name, Is.EqualTo(types[0].Name));
        }

        [Test]
        public async Task Handle_NoTypes_ReturnsEmptyList()
        {
            _typeRepository.GetAllTypesAsync().Returns(Enumerable.Empty<ProductType>());

            var result = await _handler.Handle(new GetAllTypesQuery(), CancellationToken.None);

            Assert.That(result, Is.Empty);
        }
    }
}
