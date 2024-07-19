using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Domain.Abstractions.Application.Services
{   
    public interface IValidatorService<T> where T : class
    {
        bool IsValid(ValidatorType validatorType, T? model = null, int? id = null);
    }
}
