namespace EmpXpo.Accounting.Domain
{
    public class Seller : EntityBase
    {
        public Seller()
        {
            CashFlows = new List<CashFlow>();
            Reports = new List<Report>();
        }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public IEnumerable<CashFlow> CashFlows { get; set; }
        public IEnumerable<Report> Reports { get; set; }
    }
}
