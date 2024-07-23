using EmpXpo.Accounting.Application;
using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using FluentValidation.Results;
using Moq;

namespace CashFlowReportApiTests.Controllers
{
    public class ReportApplicationTests
    {
        private readonly Mock<INotifierService> _notifierServiceMock;
        private readonly Mock<IRepository<Report>> _reportRepositoryMock;
        private readonly IValidatorService<Report> _validatorService;
        private readonly IReportApplication _reportApplication;

        public ReportApplicationTests()
        {
            _notifierServiceMock = new Mock<INotifierService>();
            _validatorService = new ReportValidatorService(_notifierServiceMock.Object);
            _reportRepositoryMock = new Mock<IRepository<Report>>();
            _reportApplication = new ReportApplication(_reportRepositoryMock.Object, _validatorService);

            // Default setup for repository queries
            _reportRepositoryMock.Setup(repo => repo.Query<DateTime>("CashFlowReportDate", null))
                .ReturnsAsync(new List<DateTime> { DateTime.Now });

            _reportRepositoryMock.Setup(repo => repo.Query<Report>("CashFlowReport", It.IsAny<object>()))
                .ReturnsAsync(new List<Report>
                {
                    new Report { Credit = 100, Debit = 50, DailyBalance = 50 }
                });
        }

        [Fact]
        public async Task ListDates_ShouldReturnNonEmptyList()
        {
            // Act
            var result = await _reportApplication.ListDates();

            // Assert
            var dates = Assert.IsAssignableFrom<IEnumerable<DateTime>>(result);
            Assert.NotEmpty(dates);
            Assert.DoesNotContain(DateTime.MinValue, dates);
        }

        [Fact]
        public async Task ListDates_WhenNoDates_ShouldReturnEmptyList()
        {
            // Arrange
            _reportRepositoryMock.Setup(repo => repo.Query<DateTime>("CashFlowReportDate", null))
                .ReturnsAsync(new List<DateTime>());

            // Act
            var result = await _reportApplication.ListDates();

            // Assert
            var dates = Assert.IsAssignableFrom<IEnumerable<DateTime>>(result);
            Assert.Empty(dates);
        }

        [Fact]
        public async Task Report_WithValidDate_ShouldReturnReport()
        {
            // Arrange
            var date = DateTime.Now;
            _reportRepositoryMock.Setup(repo => repo.Get("CashFlowReport", It.IsAny<object>()))
                                 .ReturnsAsync(new Report());

            // Act
            var result = await _reportApplication.Report(date);

            // Assert
            var report = Assert.IsType<Report>(result);
            Assert.NotNull(report);
            _reportRepositoryMock.Verify(x => x.Get("CashFlowReport", It.IsAny<object>()));
        }

        [Fact]
        public async Task Report_WhenNoReports_ShouldReturnEmptyList()
        {
            // Arrange
            var date = new DateTime(2024, 7, 19);
            _reportRepositoryMock.Setup(repo => repo.Get("CashFlowReport", It.IsAny<object>()))
                                 .ReturnsAsync(new Report());

            // Act
            var result = await _reportApplication.Report(date);

            // Assert
            var report = Assert.IsType<Report>(result);
            Assert.NotNull(report);
            _reportRepositoryMock.Verify(x => x.Get("CashFlowReport", It.IsAny<object>()));
        }

        [Fact]
        public async Task Report_WhenReportsHaveZeroValues_ShouldReturnReportsWithZeroValues()
        {
            // Arrange
            _reportRepositoryMock.Setup(repo => repo.Get("CashFlowReport", It.IsAny<object>()))
                .ReturnsAsync(new Report { Credit = 0, Debit = 0, DailyBalance = 0 });

            var date = DateTime.Now;

            // Act
            var result = await _reportApplication.Report(date);

            // Assert
            var report = Assert.IsType<Report>(result);

            Assert.Equal(0, report.Credit);
            Assert.Equal(0, report.Debit);
            Assert.Equal(0, report.DailyBalance);
        }

        [Fact]
        public async Task Report_WithInvalidDate_ShouldReturnReportWithNoDataAndLogNotification()
        {
            // Arrange
            var invalidDate = DateTime.MinValue;
            _notifierServiceMock.Setup(notifier => notifier.Add(It.IsAny<ValidationResult>())).Verifiable();

            //_validatorService.Setup(validator => validator.IsValid<DateTime>(It.IsAny<string>(), It.IsAny<DateTime>()))
            //                     .ReturnsAsync(false);
            // Act
            var result = await _reportApplication.Report(invalidDate);

            // Assert
            var report = Assert.IsType<Report>(result);
            Assert.Equal(0, report.Credit);
            Assert.Equal(0, report.Debit);
            Assert.Equal(0, report.DailyBalance);
            _notifierServiceMock.Verify(notifier => notifier.Add(It.IsAny<ValidationResult>()), Times.Once);
        }

        [Fact]
        public async Task Report_WithValidDate_ShouldReturnExpectedReports()
        {
            // Arrange
            var validDate = new DateTime(2024, 7, 19);
            var expectedReports = new Report { Credit = 100, Debit = 50, DailyBalance = 50 };

            _reportRepositoryMock.Setup(repo => repo.Get("CashFlowReport", It.IsAny<object>()))
                                 .ReturnsAsync(expectedReports);

            //_validatorService.Setup(validator => validator.IsValid<DateTime>(It.IsAny<string>(), It.IsAny<DateTime>()))
            //                     .ReturnsAsync(true);

            // Act
            var result = await _reportApplication.Report(validDate);

            // Assert
            var reports = Assert.IsType<Report>(result);
            Assert.Equal(expectedReports.Credit, reports.Credit);
            Assert.Equal(expectedReports.Debit, reports.Debit);
            Assert.Equal(expectedReports.DailyBalance, reports.DailyBalance);
        }

        [Fact]
        public async Task Report_WithValidDate_ShouldReturnCorrectReport()
        {
            // Arrange
            var validDate = new DateTime(2024, 7, 19);
            var expectedReport = new Report { Credit = 200, Debit = 100, DailyBalance = 100 };
            //_validatorService.Setup(validator => validator.IsValid<DateTime>(It.IsAny<string>(), It.IsAny<DateTime>()))
            //    .ReturnsAsync(true);
            _reportRepositoryMock.Setup(repo => repo.Get("CashFlowReport", It.IsAny<object>()))
                .ReturnsAsync(expectedReport);

            // Act
            var result = await _reportApplication.Report(validDate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReport.Credit, result.Credit);
            Assert.Equal(expectedReport.Debit, result.Debit);
            Assert.Equal(expectedReport.DailyBalance, result.DailyBalance);
        }

        [Fact]
        public async Task ListDates_ShouldReturnDistinctDates()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime(2024, 7, 19),
                new DateTime(2024, 7, 20),
                new DateTime(2024, 7, 19) // Data duplicada
            };
            _reportRepositoryMock.Setup(repo => repo.Query<DateTime>("CashFlowReportDate", null))
                .ReturnsAsync(dates);

            // Act
            var result = await _reportApplication.ListDates();

            // Assert
            var distinctDates = Assert.IsAssignableFrom<IEnumerable<DateTime>>(result);
            Assert.Equal(2, distinctDates.Count()); // Deve conter apenas datas distintas
            Assert.Contains(new DateTime(2024, 7, 19), distinctDates);
            Assert.Contains(new DateTime(2024, 7, 20), distinctDates);
        }


        [Fact]
        public async Task Report_WithStartAndEndDateAtLimits_ShouldReturnExpectedResults()
        {
            // Arrange
            var startDate = new DateTime(1900, 1, 1);
            var endDate = new DateTime(2100, 12, 31);
            var report = new Report { Credit = 500, Debit = 250, DailyBalance = 250 };
            //_validatorService.Setup(validator => validator.IsValid<DateTime>(It.IsAny<string>(), It.IsAny<DateTime>()))
            //    .ReturnsAsync(true);

            _reportRepositoryMock.Setup(repo => repo.Get("CashFlowReport", It.IsAny<object>()))
                .ReturnsAsync(report);

            // Act
            var resultStartDate = await _reportApplication.Report(startDate);
            var resultEndDate = await _reportApplication.Report(endDate);

            // Assert
            Assert.Equal(report.Credit, resultStartDate.Credit);
            Assert.Equal(report.Debit, resultStartDate.Debit);
            Assert.Equal(report.DailyBalance, resultStartDate.DailyBalance);
            Assert.Equal(report.Credit, resultEndDate.Credit);
            Assert.Equal(report.Debit, resultEndDate.Debit);
            Assert.Equal(report.DailyBalance, resultEndDate.DailyBalance);
        }
    }
}
