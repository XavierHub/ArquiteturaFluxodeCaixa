using FluentValidation.Results;

namespace EmpXpo.Accounting.Domain.Abstractions.Services
{
    public interface INotifierService
    {
        IReadOnlyCollection<Notifier> Notifications();
        bool HasNotifications();        
        void Add(string key, string message);
        void Add(ValidationResult validationResult);
    }
}
