namespace EmpXpo.Accounting.Domain
{
    public class SellerModel
    {
        /// <summary>
        /// Seller Id
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }

        /// <summary>
        /// Seller Name
        /// </summary>
        /// <example>Jonh Doe</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Seller E-mail
        /// </summary>
        /// <example>jonh_doe@email.com</example>
        public string Email { get; set; } = string.Empty;
    }
}
