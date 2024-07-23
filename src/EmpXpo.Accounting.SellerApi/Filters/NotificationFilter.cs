using EmpXpo.Accounting.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace EmpXpo.Accounting.CashFlowSellerApi.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly INotifierService _notifierService;
        private const string NOTIFICATION = "Notification";

        public NotificationFilter(INotifierService notifierService)
        {
            _notifierService = notifierService;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notifierService.HasNotifications())
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = "application/json";

                var notifications = JsonConvert.SerializeObject(new { message = "Notification", details = _notifierService.Notifications() });
                await context.HttpContext.Response.WriteAsync(notifications);

                return;
            }
            await next();
        }
    }
}
