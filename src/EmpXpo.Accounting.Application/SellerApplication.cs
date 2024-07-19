using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Application
{
    public class SellerApplication : ApplicationBase<Seller>, ISellerApplication
    {
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IValidatorService<Seller> _sellerValidatorService;

        public SellerApplication(IRepository<Seller> sellerFlowRepository, IValidatorService<Seller> sellerValidatorService) 
            : base(sellerFlowRepository, sellerValidatorService)
        {
            _sellerRepository = sellerFlowRepository;
            _sellerValidatorService = sellerValidatorService;
        }

        public override async Task<Seller> Create(Seller model)
        {
            model.CreatedOn = DateTime.Now;
            return await base.Create(model);
        }

        public override async Task<bool> Update(int id, Seller model)
        {
            if (!_sellerValidatorService.IsValid(ValidatorType.Update, model, id))
                return false;

            var entity = await _sellerRepository.GetById(id);
            if(entity.Id == 0)
                return false;

            entity.Name = model.Name;
            entity.Email = model.Email;

            return await _sellerRepository.Update(entity);
        }
    }
}
