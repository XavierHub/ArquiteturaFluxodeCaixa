using EmpXpo.Accounting.Application.Services;
using EmpXpo.Accounting.Application.Services.Validators;
using EmpXpo.Accounting.Domain;
using EmpXpo.Accounting.Domain.Abstractions.Application;
using EmpXpo.Accounting.Domain.Abstractions.Application.Services;
using EmpXpo.Accounting.Domain.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmpXpo.Accounting.Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICashFlowApplication, CashFlowApplication>();
            services.AddScoped<IReportApplication, ReportApplication>();
            services.AddScoped<ISellerApplication, SellerApplication>();
            services.AddScoped<INotifierService, NotifierService>();
            services.AddScoped(typeof(IValidatorService<CashFlow>), typeof(CashFlowValidatorService));
            services.AddScoped(typeof(IValidatorService<Seller>), typeof(SellerValidatorService));
            services.AddScoped(typeof(IValidatorService<Report>), typeof(ReportValidatorService));
            services.AddScoped(typeof(IGenericValidatorService<>), typeof(GenericValidatorService<>));
            

            return services;
        }
    }
}
