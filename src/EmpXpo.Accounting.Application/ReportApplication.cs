using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;

namespace EmpXpo.Accounting.Application
{
    public class ReportApplication : ApplicationBase<Report>, IReportApplication
    {
        private readonly IRepository<Report> _cashFlowReportRepository;
        private readonly IValidatorService<Report> _validatorService;

        public ReportApplication(IRepository<Report> reportReportRepository,
                                 IValidatorService<Report> validatorService
                                ) : base(reportReportRepository, validatorService)
        {
            _validatorService = validatorService;
            _cashFlowReportRepository = reportReportRepository;
        }

        public async Task<IEnumerable<DateTime>> ListDates()
        {
            var result = await _cashFlowReportRepository.Query<DateTime>("CashFlowReportDate");

            return result.Distinct();
        }

        public async Task<Report> Report(DateTime date)
        {
            var result = await _validatorService.IsValidValue(nameof(date), date, (date) => date is DateTime dt && dt > DateTime.MinValue);
            var report = new Report();

            if (result)
            {
                var startDate = new DateTime(date.Year, date.Month, date.Day);
                var endDate = startDate.AddDays(1);

                report = await _cashFlowReportRepository.Get("CashFlowReport", new { startDate, endDate });
                report.Reference = date;
            }

            return report;
        }
    }
}
