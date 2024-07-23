using AutoMapper;
using EmpXpo.Accounting.CashFlowSellerApi.Controllers;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace SellerApi.Tests
{
    public class SellerControllerTests
    {
        private readonly Mock<ISellerApplication> _mockSellerApplication;
        private readonly CashFlowSellerController _controller;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly IMapper _mapper;

        public SellerControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SellerModel, Seller>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _mockSellerApplication = new Mock<ISellerApplication>();
            _mockUrlHelper = new Mock<IUrlHelper>();

            _controller = new CashFlowSellerController(_mapper, _mockSellerApplication.Object)
            {
                Url = _mockUrlHelper.Object
            };

            _mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                          .Returns("http://localhost:5000/api/Sellers/1");
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult_WhenModelIsValid()
        {
            // Arrange
            var sellerModel = new SellerModel { Id = 1, Name = "Test Seller", Email = "test@example.com" };
            var seller = new Seller { Id = 1, Name = "Test Seller", Email = "test@example.com" };
            _mockSellerApplication.Setup(s => s.Create(It.IsAny<Seller>())).ReturnsAsync(seller);

            // Act
            var result = await _controller.Create(sellerModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.IsType<SellerModel>(createdResult.Value);
            _mockSellerApplication.Verify(s => s.Create(It.IsAny<Seller>()), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(new SellerModel());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenModelIsValid()
        {
            // Arrange
            var sellerModel = new SellerModel { Id = 1, Name = "Updated Seller", Email = "updated@example.com" };
            var seller = new Seller { Id = 1, Name = "Updated Seller", Email = "updated@example.com" };
            _mockSellerApplication.Setup(s => s.Update(It.IsAny<int>(), It.IsAny<Seller>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, sellerModel);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockSellerApplication.Verify(s => s.Update(It.IsAny<int>(), It.IsAny<Seller>()), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Update(1, new SellerModel());

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WhenSellerExists()
        {
            // Arrange
            var seller = new Seller { Id = 1, Name = "Test Seller", Email = "test@example.com" };
            var sellerModel = new SellerModel { Id = 1, Name = "Test Seller", Email = "test@example.com" };
            _mockSellerApplication.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(seller);

            // Act
            var result = await _controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<SellerModel>(okResult.Value);
            _mockSellerApplication.Verify(s => s.Get(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenSellerDoesNotExist()
        {
            // Arrange
            _mockSellerApplication.Setup(s => s.Get(It.IsAny<int>())).ReturnsAsync(new Seller());

            // Act
            var result = await _controller.Get(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WhenSellersExist()
        {
            // Arrange
            var sellers = new List<Seller>
            {
                new Seller { Id = 1, Name = "Test Seller 1", Email = "test1@example.com" },
                new Seller { Id = 2, Name = "Test Seller 2", Email = "test2@example.com" }
            };
            var sellerModels = new List<SellerModel>
            {
                new SellerModel { Id = 1, Name = "Test Seller 1", Email = "test1@example.com" },
                new SellerModel { Id = 2, Name = "Test Seller 2", Email = "test2@example.com" }
            };
            _mockSellerApplication.Setup(s => s.GetAll()).ReturnsAsync(sellers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<SellerModel>>(okResult.Value);
            _mockSellerApplication.Verify(s => s.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFound_WhenNoSellersExist()
        {
            // Arrange
            _mockSellerApplication.Setup(s => s.GetAll()).ReturnsAsync(new List<Seller>());

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockSellerApplication.Setup(s => s.Delete(It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockSellerApplication.Verify(s => s.Delete(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Id", "Required");

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
