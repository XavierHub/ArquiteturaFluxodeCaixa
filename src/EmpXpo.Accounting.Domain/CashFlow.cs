using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Domain
{
    public class CashFlow : EntityBase
    {
        public CashFlow()
        {
            Seller = new Seller();
        }

        public int SellerId { get; set; }
        public CashFlowType Type { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public Seller Seller { get; set; }
    }
}
