using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Application
{
    public class CashFlowApplication : ApplicationBase<CashFlow>, ICashFlowApplication
    {
        private readonly IRepository<CashFlow> _cashFlowRepository;
        private readonly IValidatorService<CashFlow> _validatorService;

        public CashFlowApplication(IRepository<CashFlow> cashFlowRepository, IValidatorService<CashFlow> validatorService) 
            : base(cashFlowRepository, validatorService)
        {
            _validatorService = validatorService;
            _cashFlowRepository = cashFlowRepository;
        }

        public override async Task<CashFlow> Create(CashFlow model)
        {
            var result = _validatorService.IsValid(ValidatorType.Create, model);

            if (result)
            {
                model.CreatedOn = DateTime.Now;
                model.Amount = model.Type == CashFlowType.Credit ? Math.Abs(model.Amount) : -Math.Abs(model.Amount);
                model.Id = Convert.ToInt32(await _cashFlowRepository.Insert(model));
            }

            return model;
        }
    }
}
