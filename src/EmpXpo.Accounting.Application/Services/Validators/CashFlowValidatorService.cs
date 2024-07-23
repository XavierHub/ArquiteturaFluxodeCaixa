using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class CashFlowValidatorService : ValidatorServiceBase<CashFlow>, IValidatorService<CashFlow>
    {
        public CashFlowValidatorService(INotifierService notifierService) : base(notifierService) { }

        public override async Task<bool> IsValidAsync(ValidatorType validatorType, CashFlow? model)
        {
            if (validatorType == ValidatorType.Create)
            {
                _validator.RuleFor(x => x.SellerId).GreaterThan(0);
                _validator.RuleFor(x => x.Type).IsInEnum().WithMessage("'{PropertyName}' has an invalid value '{PropertyValue}'. The available values are 0 for Debit and 1 for Credit");
                _validator.RuleFor(x => x.Description).NotNull().NotEmpty().MaximumLength(100);
                _validator.RuleFor(x => x.Amount).GreaterThan(0);
            }
            return await base.IsValidAsync(validatorType, model);
        }
    }
}
