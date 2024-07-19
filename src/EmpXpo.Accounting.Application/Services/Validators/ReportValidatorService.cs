using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using EmpXpo.Accounting.Domain.Enumerators;
using FluentValidation;

namespace EmpXpo.Accounting.Application.Services.Validators
{
    public class ReportValidatorService: ValidatorServiceBase<Report>, IValidatorService<Report>
    {
        public ReportValidatorService(INotifierService notifierService) : base(notifierService){}

        public override bool IsValid(ValidatorType validatorType, Report? model=null, int? id=null)
        {
            return base.IsValid(validatorType, model, id);
        }
    }
}
