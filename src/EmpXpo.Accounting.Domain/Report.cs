namespace EmpXpo.Accounting.Domain
{
    public class Report : EntityBase
    {
        public Report()
        {
            Seller = new Seller();
        }
        public int SellerId { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double DailyBalance { get; set; }
        public DateTime Reference { get; set; }
        public DateTime ProcessingDate { get; set; }
        public Seller Seller { get; set; }
    }
}
