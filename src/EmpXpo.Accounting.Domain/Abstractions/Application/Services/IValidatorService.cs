using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Domain.Abstractions.Application.Services
{
    public interface IValidatorService<T> where T : class
    {
        Task<bool> IsValidAsync(ValidatorType validatorType, T? model);
        Task<bool> IsValidValue<TValue>(string name, TValue? value, Func<TValue, bool> validationRule, string errorMessage = "The '{PropertyName}' has an invalid value");
    }
}
