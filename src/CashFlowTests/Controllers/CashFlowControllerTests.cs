using AutoMapper;
using EmpXpo.Accounting.CashFlowApi;
using EmpXpo.Accounting.CashFlowApi.Controllers;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Enumerators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace EmpXpo.Accounting.Tests
{
    public class CashFlowControllerTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUrlHelper> _mockUrlHelper;
        private readonly Mock<ICashFlowApplication> _cashFlowApplicationMock;
        private readonly CashFlowController _controller;

        public CashFlowControllerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _mockUrlHelper = new Mock<IUrlHelper>();
            _cashFlowApplicationMock = new Mock<ICashFlowApplication>();
            _controller = new CashFlowController(_mapperMock.Object, _cashFlowApplicationMock.Object)
            {
                Url = _mockUrlHelper.Object
            };

            _mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                          .Returns("http://localhost:5000/api/Sellers/1");
        }

        [Fact]
        public async Task Create_ValidModel_ReturnsCreatedResult()
        {
            // Arrange
            var model = new CashFlowModel
            {
                Id = 1,
                SellerId = 1,
                Type = 1,
                Amount = 100.0,
                Description = "Valid Description"
            };

            var cashFlow = new CashFlow
            {
                Id = 1,
                SellerId = 1,
                Type = CashFlowType.Credit,
                Amount = 100.0,
                Description = "Valid Description",
                CreatedOn = DateTime.Now
            };

            _mapperMock.Setup(m => m.Map<CashFlow>(model)).Returns(cashFlow);
            _cashFlowApplicationMock.Setup(a => a.Create(It.IsAny<CashFlow>())).ReturnsAsync(cashFlow);
            _mapperMock.Setup(m => m.Map<CashFlowModel>(cashFlow)).Returns(model);

            // Act
            var result = await _controller.Create(model) as CreatedResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.Create(new CashFlowModel()) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Create_ApplicationReturnsInvalidCashFlow_ReturnsBadRequest()
        {
            // Arrange
            var model = new CashFlowModel
            {
                SellerId = 1,
                Type = 1,
                Amount = 100.0,
                Description = "Valid Description"
            };

            _mapperMock.Setup(m => m.Map<CashFlow>(model)).Returns(new CashFlow());
            _cashFlowApplicationMock.Setup(a => a.Create(It.IsAny<CashFlow>())).ReturnsAsync(new CashFlow { Id = 0 });

            // Act
            var result = await _controller.Create(model) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Create_ApplicationThrowsException_ReturnsStatusCode500()
        {
            // Arrange
            var model = new CashFlowModel
            {
                SellerId = 1,
                Type = 1,
                Amount = 100.0,
                Description = "Valid Description"
            };

            _mapperMock.Setup(m => m.Map<CashFlow>(model)).Returns(new CashFlow());
            _cashFlowApplicationMock.Setup(a => a.Create(It.IsAny<CashFlow>())).ThrowsAsync(new Exception());

            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await _controller.Create(model));
        }
    }
}
