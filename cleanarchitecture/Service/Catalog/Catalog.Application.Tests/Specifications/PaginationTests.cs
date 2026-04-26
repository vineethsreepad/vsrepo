using Catalog.Core.Specifications;
using NUnit.Framework;

namespace Catalog.Application.Tests.Specifications
{
    [TestFixture]
    public class PaginationTests
    {
        [Test]
        public void Constructor_SetsAllProperties()
        {
            var data = new List<string> { "a", "b" }.AsReadOnly();

            var pagination = new Pagination<string>(2, 10, 50, data);

            Assert.That(pagination.PageIndex, Is.EqualTo(2));
            Assert.That(pagination.PageSize, Is.EqualTo(10));
            Assert.That(pagination.Count, Is.EqualTo(50));
            Assert.That(pagination.Data, Has.Count.EqualTo(2));
        }

        [Test]
        public void DefaultConstructor_PropertiesAreDefault()
        {
            var pagination = new Pagination<string>();

            Assert.That(pagination.PageIndex, Is.EqualTo(0));
            Assert.That(pagination.PageSize, Is.EqualTo(0));
            Assert.That(pagination.Count, Is.EqualTo(0));
            Assert.That(pagination.Data, Is.Null);
        }
    }
}
