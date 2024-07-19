namespace EmpXpo.Accounting.Domain
{
    public class ReportModel: EntityBase
    {        
        /// <summary>
        /// Total Debit
        /// </summary>
        /// <example>100</example>
        public double Debit { get; set; }

        /// <summary>
        /// Total Credit
        /// </summary>
        /// <example>-30</example>
        public double Credit { get; set; }

        /// <summary>
        /// Daily Balance
        /// </summary>
        /// <example>70</example>
        public double DailyBalance { get; set; }

        /// <summary>
        /// Date the report was generated
        /// </summary>
        /// <example>70</example>
        public DateTime Reference { get; set; }

        /// <summary>
        /// Date and time the report was generated
        /// </summary>
        /// <example>70</example>
        public DateTime ProcessingDate { get; set; }        
    }
}
