using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;
using FluentValidation.Results;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public abstract class ValidatorServiceBase<T>
    {
        protected readonly InlineValidator<T> _validator;
        private readonly INotifierService _notifierService;

        protected ValidatorServiceBase(INotifierService notifierService)
        {
            _validator = new InlineValidator<T>();
            _notifierService = notifierService;
        }

        public virtual async Task<bool> IsValidAsync(ValidatorType validatorType, T? model)
        {
            if (model == null)
            {
                _notifierService.Add(new ValidationResult(new[] { new ValidationFailure("", "Model is null") }));
                return false;
            }

            var validateResultModel = await _validator.ValidateAsync(model);
            _notifierService.Add(validateResultModel);

            return validateResultModel.IsValid;
        }

        public async Task<bool> IsValidValue<TValue>(string name, TValue? value, Func<TValue, bool> validationRule, string errorMessage = "The '{PropertyName}' has an invalid value")
        {
            var validator = new InlineValidator<TValue>();
            if (value == null)
            {
                _notifierService.Add(new ValidationResult(new[] { new ValidationFailure(name, "The '{PropertyName}' property cannot be null") }));
                return false;
            }

            validator.RuleFor(x => x)
                .Must(validationRule)
                .WithName(name)
                .WithMessage(errorMessage);

            var validateResultModel = await validator.ValidateAsync(value);
            _notifierService.Add(validateResultModel);

            return validateResultModel.IsValid;
        }
    }
}
