using Catalog.Core.Specifications;
using NUnit.Framework;

namespace Catalog.Application.Tests.Specifications
{
    [TestFixture]
    public class CatalogSpecParamsTests
    {
        [Test]
        public void PageSize_Default_Is10()
        {
            var spec = new CatalogSpecParams();

            Assert.That(spec.PageSize, Is.EqualTo(10));
        }

        [Test]
        public void PageIndex_Default_Is1()
        {
            var spec = new CatalogSpecParams();

            Assert.That(spec.PageIndex, Is.EqualTo(1));
        }

        [Test]
        public void PageSize_ExceedsMax_ClampedTo70()
        {
            var spec = new CatalogSpecParams { PageSize = 100 };

            Assert.That(spec.PageSize, Is.EqualTo(70));
        }

        [Test]
        public void PageSize_WithinRange_AcceptedAsIs()
        {
            var spec = new CatalogSpecParams { PageSize = 50 };

            Assert.That(spec.PageSize, Is.EqualTo(50));
        }
    }
}
