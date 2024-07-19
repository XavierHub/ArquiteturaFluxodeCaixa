using EmpXpo.Accounting.Domain.Enumerators;

namespace EmpXpo.Accounting.Domain.Abstractions.Application.Services
{   
    public interface IGenericValidatorService<T>
    {
        bool IsValid(T? model);
    }
}
