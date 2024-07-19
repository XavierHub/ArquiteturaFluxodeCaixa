using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public abstract class ValidatorServiceBase<T> : AbstractValidator<T>
    {
        protected readonly InlineValidator<T> _validator;
        protected readonly InlineValidator<int?> _validatorId;
        protected readonly INotifierService _notifierService;        

        protected ValidatorServiceBase(INotifierService notifierService)
        {
            _validator = new InlineValidator<T>();
            _validatorId = new InlineValidator<int?>();
            _notifierService = notifierService;

            _validatorId.RuleFor(x => x).NotNull().GreaterThan(0).WithName("Id"); ;
        }

        public virtual bool IsValid(ValidatorType validatorType, T? model, int? id)
        {
            var isValid = true;
            if (validatorType == ValidatorType.Get || validatorType == ValidatorType.Update || validatorType == ValidatorType.Delete)
            {
                var validateResultId =  _validatorId.Validate(id);
                isValid = validateResultId.IsValid;
                _notifierService.Add(validateResultId);
            }

            if (model != null)
            {
                var validateResultModel = _validator.Validate(model);
                _notifierService.Add(validateResultModel);

                isValid = isValid && validateResultModel.IsValid;
            }

            return isValid;
        }
    }
}
