using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Repositories;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Application
{
    public class CashFlowApplication : ApplicationBase<CashFlow>, ICashFlowApplication
    {
        private readonly IRepository<CashFlow> _cashFlowRepository;
        private readonly IRepository<Seller> _sellerRepository;
        private readonly IValidatorService<CashFlow> _validatorService;
        private readonly INotifierService _notifierService;

        public CashFlowApplication(
            INotifierService notifierService,
            IRepository<CashFlow> cashFlowRepository,
            IRepository<Seller> sellerRepository,
            IValidatorService<CashFlow> validatorService)
            : base(cashFlowRepository, validatorService)
        {
            _validatorService = validatorService;
            _sellerRepository = sellerRepository;
            _notifierService = notifierService;
            _cashFlowRepository = cashFlowRepository;
        }

        public override async Task<CashFlow> Create(CashFlow model)
        {
            if (!await _validatorService.IsValidAsync(ValidatorType.Create, model))
                return new CashFlow();

            var seller = await _sellerRepository.GetById(model.SellerId);
            if (seller.Id == 0)
            {
                _notifierService.Add("Seller", $"The specified SellerId '{model.SellerId}' does not exist.");
                return new CashFlow();
            }

            model.CreatedOn = DateTime.Now;
            model.Amount = model.Type == CashFlowType.Credit ? Math.Abs(model.Amount) : -Math.Abs(model.Amount);
            model.Id = Convert.ToInt32(await _cashFlowRepository.Insert(model));

            return model;
        }
    }
}
