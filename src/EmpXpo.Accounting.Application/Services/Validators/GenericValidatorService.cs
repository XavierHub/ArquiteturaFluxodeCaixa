using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using FluentValidation;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class GenericValidatorService<T> : AbstractValidator<T>, IGenericValidatorService<T>
    {
        protected readonly InlineValidator<T> _validator;
        protected readonly INotifierService _notifierService;

        public GenericValidatorService(INotifierService notifierService)
        {
            _validator = new InlineValidator<T>();
            _notifierService = notifierService;
        }

        public virtual bool IsValid(T? model)
        {
            if (Nullable.GetUnderlyingType(typeof(T)) != null)
            {
                RuleFor(x => x).NotNull();
            }           
            else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
            {
                RuleFor(x => (DateTime)(object)x!).GreaterThan(DateTime.MinValue).WithMessage("'{PropertyName}' must be a valid date.");
            }            

            var validateResultModel = _validator.Validate(model);
            _notifierService.Add(validateResultModel);

            return validateResultModel.IsValid;
        }

    }
}
