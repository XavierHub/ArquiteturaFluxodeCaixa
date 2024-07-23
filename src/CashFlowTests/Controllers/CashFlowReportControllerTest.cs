using AutoMapper;
using EmpXpo.Accounting.CashFlowApi.Controllers;
using EmpXpo.Accounting.CashFlowApi.Models;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace EmpXpo.Accounting.CashFlowApi.Tests
{
    public class CashFlowReportControllerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IReportApplication> _mockReportApplication;
        private readonly CashFlowReportController _controller;

        public CashFlowReportControllerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockReportApplication = new Mock<IReportApplication>();
            _controller = new CashFlowReportController(_mockMapper.Object, _mockReportApplication.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithDates()
        {
            // Arrange
            var dates = new List<DateTime> { DateTime.Now, DateTime.Now.AddDays(-1) };
            _mockReportApplication.Setup(x => x.ListDates()).ReturnsAsync(dates);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dates, okResult.Value);
            _mockReportApplication.Verify(x => x.ListDates(), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNoDates()
        {
            // Arrange
            var dates = new List<DateTime>();
            _mockReportApplication.Setup(x => x.ListDates()).ReturnsAsync(dates);

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockReportApplication.Verify(x => x.ListDates(), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.Get();

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetByDate_ReturnsOkResult_WithReport()
        {
            // Arrange
            var date = DateTime.Now;
            var report = new Report { ProcessingDate = date };
            var reportModel = new ReportModel { ProcessingDate = date };
            _mockReportApplication.Setup(x => x.Report(date)).ReturnsAsync(report);
            _mockMapper.Setup(m => m.Map<ReportModel>(It.IsAny<Report>())).Returns(reportModel);

            // Act
            var result = await _controller.Get(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(reportModel, okResult.Value);
            _mockReportApplication.Verify(x => x.Report(date), Times.Once);
        }

        [Fact]
        public async Task GetByDate_ReturnsNotFound_WhenReportNotFound()
        {
            // Arrange
            var date = DateTime.Now;
            var report = new Report { ProcessingDate = DateTime.MinValue };
            _mockReportApplication.Setup(x => x.Report(date)).ReturnsAsync(report);

            // Act
            var result = await _controller.Get(date);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            _mockReportApplication.Verify(x => x.Report(date), Times.Once);
        }

        [Fact]
        public async Task GetByDate_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");
            var date = DateTime.Now;

            // Act
            var result = await _controller.Get(date);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetByDate_ReturnsBadRequest_WhenDateIsInvalid()
        {
            // Arrange
            var date = DateTime.MinValue;

            // Act
            var result = await _controller.Get(date);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _mockReportApplication.Setup(x => x.ListDates()).ThrowsAsync(new Exception("Test exception"));

            // Act
            await Assert.ThrowsAsync<Exception>(async () => await _controller.Get());

            // Assert
            _mockReportApplication.Verify(x => x.ListDates(), Times.Once);
        }

        [Fact]
        public async Task GetByDate_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var date = DateTime.Now;
            _mockReportApplication.Setup(x => x.Report(date)).ThrowsAsync(new Exception("Test exception"));

            // Assert
            await Assert.ThrowsAsync<Exception>(async () => await _controller.Get(date));

            // Assert
            _mockReportApplication.Verify(x => x.Report(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task GetByDate_ReturnsBadRequest_WhenDateFormatIsInvalid()
        {
            // Arrange
            var date = DateTime.MinValue;

            // Act
            var result = await _controller.Get(date);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetByDate_ReturnsOkResult_WhenDateIsToday()
        {
            // Arrange
            var date = DateTime.Today;
            var report = new Report { ProcessingDate = date };
            var reportModel = new ReportModel { ProcessingDate = date };
            _mockReportApplication.Setup(x => x.Report(date)).ReturnsAsync(report);
            _mockMapper.Setup(m => m.Map<ReportModel>(It.IsAny<Report>())).Returns(reportModel);

            // Act
            var result = await _controller.Get(date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(reportModel, okResult.Value);
            _mockReportApplication.Verify(x => x.Report(date), Times.Once);
        }
    }
}
