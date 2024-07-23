using EmpXpo.Accounting.Domain.Abstractions.Domain;

namespace EmpXpo.Accounting.Domain
{
    public class EntityBase : IEntity
    {
        public int Id { get; set; }
    }
}
