using EmpXpo.Accounting.Domain.Abstractions.Domain;

namespace EmpXpo.Accounting.Domain.Abstractions.Application
{
    public interface ISellerApplication
    {
        public Task<Seller> Create(Seller entity);
        public Task<bool> Update(int id, Seller entity);
        public Task<bool> Delete(int id);
        public Task<Seller> Get(int id);        
        public Task<IEnumerable<Seller>> GetAll();
    }
}
