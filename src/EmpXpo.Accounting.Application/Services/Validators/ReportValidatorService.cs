using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class ReportValidatorService : ValidatorServiceBase<Report>, IValidatorService<Report>
    {
        public ReportValidatorService(INotifierService notifierService) : base(notifierService) { }

        public override async Task<bool> IsValidAsync(ValidatorType validatorType, Report? model)
        {
            if (validatorType == ValidatorType.Create)
            {
                _validator.RuleFor(x => x.SellerId).GreaterThan(0);
                _validator.RuleFor(x => x.Credit).GreaterThan(0);
            }

            return await base.IsValidAsync(validatorType, model);
        }
    }
}
