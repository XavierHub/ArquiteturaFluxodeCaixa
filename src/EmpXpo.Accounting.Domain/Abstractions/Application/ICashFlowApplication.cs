namespace EmpXpo.Accounting.Domain.Abstractions.Application
{
    public interface ICashFlowApplication
    {
        public Task<CashFlow> Create(CashFlow model);        
    }
}
