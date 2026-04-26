using AutoFixture;
using Catalog.Application.Mappers;
using Catalog.Core.Entities;
using NUnit.Framework;

namespace Catalog.Application.Tests.Mappers
{
    [TestFixture]
    public class TypesMapperTests
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
            var productType = _fixture.Create<ProductType>();

            var result = productType.ToResponse();

            Assert.That(result.Id, Is.EqualTo(productType.Id));
            Assert.That(result.Name, Is.EqualTo(productType.Name));
        }

        [Test]
        public void ToResponseList_MultipleTypes_MapsAll()
        {
            var types = _fixture.CreateMany<ProductType>(3).ToList();

            var result = types.ToResponseList();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo(types[0].Id));
            Assert.That(result[1].Name, Is.EqualTo(types[1].Name));
        }
    }
}
