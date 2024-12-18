using Bogus;
using MassTransit;
using Moq;
using PersonalShop.Data;
using PersonalShop.Data.Repositories;
using PersonalShop.Features.Products;
using PersonalShop.Features.Products.Dtos;
using PersonalShop.Tests.Application.Fixture;


namespace PersonalShop.Tests.Application.Services.ProductServiceTests
{
    public class ProductServiceTests : IClassFixture<DbFixture>
    {
        private readonly DbFixture _fixture;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _productRepository;
        private readonly ProductRepository _productQueryRepository;

        public ProductServiceTests(DbFixture fixture)
        {
            _fixture = fixture;
            _unitOfWork = new(_fixture.DbContext);
            _productRepository = new(_fixture.DbContext);
            _productQueryRepository = new(_fixture.DbContext);
        }

        [Fact]
        public async void AddProduct_Should_Return_True_When_ProductAdded()
        {
            //Arrange
            var mockBus = new Mock<IBus>();
            var productService = new ProductService(_productRepository, _productQueryRepository, _unitOfWork, mockBus.Object);

            var userId = Guid.NewGuid().ToString();

            var productDto = new Faker<CreateProductDto>()
                .RuleFor(x => x.Name, f => f.Commerce.ProductName())
                .RuleFor(x => x.Name, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(2000, 1000000)));


            //Act
            var result = await productService.CreateProductAsync(productDto, userId);

            //Assert
            Assert.True(result.IsSuccess);
        }
    }
}