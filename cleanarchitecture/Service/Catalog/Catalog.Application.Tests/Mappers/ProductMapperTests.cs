using AutoFixture;
using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using NUnit.Framework;

namespace Catalog.Application.Tests.Mappers
{
    [TestFixture]
    public class ProductMapperTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void ToResponse_ValidProduct_MapsAllFields()
        {
            var product = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();

            var result = product.ToResponse();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(product.Id));
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Summary, Is.EqualTo(product.Summary));
            Assert.That(result.Description, Is.EqualTo(product.Description));
            Assert.That(result.Brand, Is.EqualTo(product.Brand));
            Assert.That(result.Type, Is.EqualTo(product.Type));
            Assert.That(result.Price, Is.EqualTo(product.Price));
            Assert.That(result.CreatedDate, Is.EqualTo(product.CreatedDate));
        }

        [Test]
        public void ToResponse_NullProduct_ReturnsNull()
        {
            Product product = null;

            var result = product.ToResponse();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ToResponseList_MultipleProducts_MapsAll()
        {
            var products = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .CreateMany(3)
                .ToList();

            var result = products.ToResponseList();

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo(products[0].Name));
            Assert.That(result[1].Name, Is.EqualTo(products[1].Name));
            Assert.That(result[2].Name, Is.EqualTo(products[2].Name));
        }

        [Test]
        public void ToResponse_Pagination_MapsPageIndexPageSizeCountAndData()
        {
            var products = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .CreateMany(2)
                .ToList();
            var pagination = new Pagination<Product>(1, 10, 100, products);

            var result = pagination.ToResponse();

            Assert.That(result.PageIndex, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(10));
            Assert.That(result.Count, Is.EqualTo(100));
            Assert.That(result.Data, Has.Count.EqualTo(2));
        }

        [Test]
        public void ToEntity_MapsCommandFieldsAndSetsCreatedDate()
        {
            var command = _fixture.Create<CreateProductCommand>();
            var brand = _fixture.Create<ProductBrand>();
            var type = _fixture.Create<ProductType>();

            var before = DateTimeOffset.UtcNow;
            var result = command.ToEntity(brand, type);
            var after = DateTimeOffset.UtcNow;

            Assert.That(result.Name, Is.EqualTo(command.Name));
            Assert.That(result.Summary, Is.EqualTo(command.Summary));
            Assert.That(result.Description, Is.EqualTo(command.Description));
            Assert.That(result.ImageFile, Is.EqualTo(command.ImageFile));
            Assert.That(result.Brand, Is.EqualTo(brand));
            Assert.That(result.Type, Is.EqualTo(type));
            Assert.That(result.Price, Is.EqualTo(command.Price));
            Assert.That(result.CreatedDate, Is.GreaterThanOrEqualTo(before));
            Assert.That(result.CreatedDate, Is.LessThanOrEqualTo(after));
        }

        [Test]
        public void ToUpdateEntity_PreservesIdAndCreatedDate()
        {
            var existingProduct = _fixture.Build<Product>()
                .With(p => p.Brand, _fixture.Create<ProductBrand>())
                .With(p => p.Type, _fixture.Create<ProductType>())
                .Create();
            var command = _fixture.Create<UpdateProductCommand>();
            var brand = _fixture.Create<ProductBrand>();
            var type = _fixture.Create<ProductType>();

            var result = command.ToUpdateEntity(existingProduct, brand, type);

            Assert.That(result.Id, Is.EqualTo(existingProduct.Id));
            Assert.That(result.CreatedDate, Is.EqualTo(existingProduct.CreatedDate));
            Assert.That(result.Name, Is.EqualTo(command.Name));
            Assert.That(result.Brand, Is.EqualTo(brand));
            Assert.That(result.Type, Is.EqualTo(type));
        }

        [Test]
        public void ToDto_ValidResponse_MapsAllFields()
        {
            var response = _fixture.Build<ProductResponse>()
                .With(r => r.Brand, _fixture.Create<ProductBrand>())
                .With(r => r.Type, _fixture.Create<ProductType>())
                .Create();

            var result = response.ToDto();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(response.Id));
            Assert.That(result.Name, Is.EqualTo(response.Name));
            Assert.That(result.Brand.Id, Is.EqualTo(response.Brand.Id));
            Assert.That(result.Brand.Name, Is.EqualTo(response.Brand.Name));
            Assert.That(result.Type.Id, Is.EqualTo(response.Type.Id));
            Assert.That(result.Type.Name, Is.EqualTo(response.Type.Name));
            Assert.That(result.Price, Is.EqualTo(response.Price));
        }

        [Test]
        public void ToDto_NullResponse_ReturnsNull()
        {
            ProductResponse response = null;

            var result = response.ToDto();

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ToCommand_MapsUpdateProductDtoToCommand()
        {
            var dto = _fixture.Build<UpdateProductDto>()
                .With(d => d.Price, 99.99m)
                .Create();
            var id = _fixture.Create<string>();

            var result = dto.ToCommand(id);

            Assert.That(result.Id, Is.EqualTo(id));
            Assert.That(result.Name, Is.EqualTo(dto.Name));
            Assert.That(result.Summary, Is.EqualTo(dto.Summary));
            Assert.That(result.Description, Is.EqualTo(dto.Description));
            Assert.That(result.ImageFile, Is.EqualTo(dto.ImageFile));
            Assert.That(result.BrandId, Is.EqualTo(dto.BrandId));
            Assert.That(result.TypeId, Is.EqualTo(dto.TypeId));
            Assert.That(result.Price, Is.EqualTo(dto.Price));
        }
    }
}
