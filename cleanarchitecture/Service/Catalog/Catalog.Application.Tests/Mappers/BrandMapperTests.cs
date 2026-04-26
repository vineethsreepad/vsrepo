using AutoFixture;
using Catalog.Application.Mappers;
using Catalog.Core.Entities;
using NUnit.Framework;

namespace Catalog.Application.Tests.Mappers
{
    [TestFixture]
    public class BrandMapperTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void ToResponse_MapsIdAndName()
        {
            var brand = _fixture.Create<ProductBrand>();

            var result = brand.ToResponse();

            Assert.That(result.Id, Is.EqualTo(brand.Id));
            Assert.That(result.Name, Is.EqualTo(brand.Name));
        }

        [Test]
        public void ToResponseList_MultipleBrands_MapsAll()
        {
            var brands = _fixture.CreateMany<ProductBrand>(3).ToList();

            var result = brands.ToResponseList();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo(brands[0].Id));
            Assert.That(result[1].Name, Is.EqualTo(brands[1].Name));
        }
    }
}
