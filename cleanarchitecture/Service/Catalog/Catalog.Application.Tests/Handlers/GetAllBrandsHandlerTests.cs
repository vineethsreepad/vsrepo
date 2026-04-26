using AutoFixture;
using Catalog.Application.Handlers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Catalog.Application.Tests.Handlers
{
    [TestFixture]
    public class GetAllBrandsHandlerTests
    {
        private Fixture _fixture;
        private IBrandRepository _brandRepository;
        private GetAllBrandsHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _brandRepository = Substitute.For<IBrandRepository>();
            _handler = new GetAllBrandsHandler(_brandRepository);
        }

        [Test]
        public async Task Handle_BrandsExist_ReturnsMappedBrandResponses()
        {
            var brands = _fixture.CreateMany<ProductBrand>(3).ToList();
            _brandRepository.GetAllBrandsAsync().Returns(brands);

            var result = await _handler.Handle(new GetAllBrandsQuery(), CancellationToken.None);

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo(brands[0].Id));
            Assert.That(result[0].Name, Is.EqualTo(brands[0].Name));
        }

        [Test]
        public async Task Handle_NoBrands_ReturnsEmptyList()
        {
            _brandRepository.GetAllBrandsAsync().Returns(Enumerable.Empty<ProductBrand>());

            var result = await _handler.Handle(new GetAllBrandsQuery(), CancellationToken.None);

            Assert.That(result, Is.Empty);
        }
    }
}
