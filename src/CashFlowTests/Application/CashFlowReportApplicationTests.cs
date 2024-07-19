using EmpXpo.Accounting.Application;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Exceptions;
using Moq;

namespace CashFlowReportApiTests.Controllers
{
    public class CashFlowApplicationTest
    {
        private readonly Mock<IRepository<Report>> _cashFlowReportRepository;
        private readonly IReportApplication _cashFlowReportApplication;

        public CashFlowApplicationTest()
        {
            _cashFlowReportRepository = new Mock<IRepository<Report>>();
            _cashFlowReportApplication = new ReportApplication(_cashFlowReportRepository.Object);

            _cashFlowReportRepository.Setup(x => x.Query<DateTime>("CashFlowReportDate", null))
                                     .Returns(Task.FromResult<IEnumerable<DateTime>>(new List<DateTime> { DateTime.Now }));

            _cashFlowReportRepository.Setup(x => x.Query<Report>("CashFlowReport", It.IsAny<object>()))
                                     .Returns(() =>
                                                  Task.Run<IEnumerable<Report>>(() =>
                                                               new List<Report> {
                                                                   new Report { Credit = 100, Debit = 50, DailyBalance = 50 }
                                                               }
                                                          )
                                             );
        }

        [Fact]
        public async Task WhenRunGet_ShouldReturnListDates()
        {
            var payload = await _cashFlowReportApplication.ListDates();
            var value = Assert.IsAssignableFrom<IEnumerable<DateTime>>(payload);

            Assert.NotNull(value);
            Assert.True(value.Count() > 0);
            Assert.DoesNotContain(DateTime.MinValue, value);
        }

        [Fact]
        public async Task WhenRunGetWithoutDatabaseDate_ShouldReturnEmptyListDates()
        {
            _cashFlowReportRepository.Setup(x => x.Query<DateTime>("CashFlowReportDate", null))
                                     .Returns(Task.FromResult<IEnumerable<DateTime>>(new List<DateTime> { DateTime.Now }));

            var payload = await _cashFlowReportApplication.ListDates();
            var value = Assert.IsAssignableFrom<IEnumerable<DateTime>>(payload);

            Assert.NotNull(value);
            Assert.True(value.Count() > 0);
            Assert.DoesNotContain(DateTime.MinValue, value);
        }

        [Fact]
        public async Task WhenRunGetWithDate_ShouldReturnReport()
        {
            var viewModel = DateTime.Now;
            var payload = await _cashFlowReportApplication.Report(viewModel);
            var value = Assert.IsAssignableFrom<IEnumerable<Report>>(payload);

            Assert.NotNull(value);
            Assert.True(value.Count() > 0);
        }

        [Fact]
        public async Task WhenRunGetWithoutDatabaseDate_ShouldReturnReportWithZeroValues()
        {
            _cashFlowReportRepository.Setup(x => x.Query<Report>("CashFlowReport", It.IsAny<object>()))
                                     .Returns(() =>
                                                  Task.Run<IEnumerable<Report>>(() =>
                                                               new List<Report> {
                                                                   new Report { Credit = 0, Debit = 0, DailyBalance = 0 }
                                                               }
                                                          )
                                             );

            var viewModel = DateTime.Now;
            var payload = await _cashFlowReportApplication.Report(viewModel);
            var values = Assert.IsAssignableFrom<IEnumerable<Report>>(payload);

            Assert.NotNull(values);
            Assert.All(values, x =>
            {
                Assert.Equal(0, x.Credit);
                Assert.Equal(0, x.Debit);
                Assert.Equal(0, x.DailyBalance);
            });
        }

        [Fact]
        public async Task WhenRunCreateWithInvalidDate_ShouldReturnException()
        {
            var viewModel = DateTime.MinValue;
            await Assert.ThrowsAsync<EmpXpo.Accounting.Domain.Exceptions.DomainException>(async () => await _cashFlowReportApplication.Report(viewModel));
        }
    }
}