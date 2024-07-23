namespace EmpXpo.Accounting.Domain.Abstractions.Application
{
    public interface IReportApplication
    {
        public Task<IEnumerable<DateTime>> ListDates();
        public Task<Report> Report(DateTime date);
    }
}
