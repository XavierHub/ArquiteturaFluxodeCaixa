namespace EmpXpo.Accounting.CashFlowApi
{
    public class CashFlowModel
    {
        /// <summary>
        /// CashFlow Id
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }

        /// <summary>
        /// Seller Id
        /// </summary>
        /// <example>1</example>
        public int SellerId { get; set; }

        /// <summary>
        /// CashFlow Type
        /// The available values ​​are 0 for Debit and 1 for Credit
        /// </summary>
        /// <example>0</example>
        public int Type { get; set; }

        /// <summary>
        /// Cash flow value must be entered without a negative or positive sign, 
        /// the type is the one who will define the sign of the transaction
        /// </summary>
        /// <example>100.50</example>
        public double Amount { get; set; }

        /// <summary>
        /// Cash flow Description
        /// </summary>
        /// <example>Purchase of xpo product</example>
        public string Description { get; set; } = string.Empty;
    }
}
