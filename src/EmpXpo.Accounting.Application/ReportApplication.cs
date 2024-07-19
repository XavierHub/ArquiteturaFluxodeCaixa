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
        private readonly IGenericValidatorService<DateTime> _genericValidatorService;

        public ReportApplication(IRepository<Report> reportReportRepository, 
                                 IValidatorService<Report> validatorService,
                                 IGenericValidatorService<DateTime> genericValidatorService
                                ) : base(reportReportRepository, validatorService)
        {
            _validatorService = validatorService;
            _genericValidatorService = genericValidatorService;
            _cashFlowReportRepository = reportReportRepository;
        }

        public async Task<IEnumerable<DateTime>> ListDates()
        {
            return await _cashFlowReportRepository.Query<DateTime>("CashFlowReportDate");
        }

        public async Task<IEnumerable<Report>> Report(DateTime model)
        {
            var result = _genericValidatorService.IsValid(model);            

            if (result)
            {
                var startDate = new DateTime(model.Year, model.Month, model.Day);
                var endDate = startDate.AddDays(1);

                return (await _cashFlowReportRepository.Query<Report>("CashFlowReport", new { startDate, endDate }));
            }

            return new List<Report>();
        }
    }
}
