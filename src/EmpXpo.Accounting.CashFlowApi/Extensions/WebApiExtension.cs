using AutoMapper;
using EmpXpo.Accounting.CashFlowApi;
using EmpXpo.Accounting.Domain;

namespace EmpXpo.Accounting.CashFlowApi.Extensions
{
    public static class WebApiExtension
    {
        public static IServiceCollection AddWebApiCashFlow(this IServiceCollection services)
        {
            var cfg = new MapperConfigurationExpression();
            cfg.CreateMap<CashFlow, CashFlowModel>().ReverseMap();

            var mapperConfig = new MapperConfiguration(cfg);
            services.AddSingleton<IMapper, Mapper>(x => new Mapper(mapperConfig));

            return services;
        }
    }
}
