using EmpXpo.Accounting.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;

namespace EmpXpo.Accounting.CashFlowApi.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private readonly INotifierService _notifierService;

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

                var notifications = JsonConvert.SerializeObject(_notifierService.Notifications());
                await context.HttpContext.Response.WriteAsync(notifications);

                return;
            }
            await next();
        }
    }
}
