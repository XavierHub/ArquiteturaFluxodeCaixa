namespace EmpXpo.Accounting.Domain.Abstractions.Application
{
    public interface IReportApplication
    {
        public Task<IEnumerable<DateTime>> ListDates();
        public Task<IEnumerable<Report>> Report(DateTime date);
    }
}
