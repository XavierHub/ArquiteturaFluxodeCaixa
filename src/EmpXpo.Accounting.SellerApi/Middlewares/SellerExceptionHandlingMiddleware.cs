﻿using System.Net;
using System.Reflection;

namespace EmpXpo.Accounting.SellerApi.Middlewares
{
    public class SellerExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SellerExceptionHandlingMiddleware> _logger;
        private readonly string appName = Assembly.GetExecutingAssembly()?.GetName()?.Name ?? "CashFlowApi";

        public SellerExceptionHandlingMiddleware(RequestDelegate next, ILogger<SellerExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{appName}|An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                message = "An unexpected error occurred. Please try again later.",
                detail = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
