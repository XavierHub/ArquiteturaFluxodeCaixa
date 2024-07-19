using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using FluentValidation.Results;

namespace EmpXpo.Accounting.Application.Services
{
    public class NotifierService : INotifierService
    {
        private readonly List<Notifier> _notifications;
        
        public NotifierService()
        {
            _notifications = new List<Notifier>();
        }
        public bool HasNotifications() => _notifications.Any();
        public IReadOnlyCollection<Notifier> Notifications() => _notifications;
        public void Add(string key, string message) => _notifications.Add(new Notifier(key, message));

        public void Add(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Add(error.ErrorCode, error.ErrorMessage);
            }
        }
    }
}
