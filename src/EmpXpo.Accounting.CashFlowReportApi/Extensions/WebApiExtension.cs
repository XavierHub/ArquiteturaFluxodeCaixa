using AutoMapper;
using EmpXpo.Accounting.CashFlowApi.Models;
using EmpXpo.Accounting.Domain;

namespace EmpXpo.Accounting.CashFlowApi.Extensions
{
    public static class WebApiExtension
    {
        public static IServiceCollection AddWebApiCashFlowReport(this IServiceCollection services)
        {
            var cfg = new MapperConfigurationExpression();
            cfg.CreateMap<Report, ReportModel>().ReverseMap();

            var mapperConfig = new MapperConfiguration(cfg);
            services.AddSingleton<IMapper, Mapper>(x => new Mapper(mapperConfig));

            return services;
        }
    }
}
