using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class SellerValidatorService: ValidatorServiceBase<Seller>, IValidatorService<Seller>
    {
        public SellerValidatorService(INotifierService notifierService):base(notifierService)
        {
            _validator.RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(80);
            _validator.RuleFor(x => x.Email).NotNull().NotEmpty().MaximumLength(50).EmailAddress();
        }

        public override bool IsValid(ValidatorType validatorType, Seller? model = null, int? id = null)
        {
            return base.IsValid(validatorType, model, id);
        }         
    }
}
