using AutoMapper;
using EmpXpo.Accounting.Domain;

namespace EmpXpo.Accounting.SellerApi.Extensions
{
    public static class WebApiExtension
    {
        public static IServiceCollection AddWebApiSeller(this IServiceCollection services)
        {
            var cfg = new MapperConfigurationExpression();
            cfg.CreateMap<SellerModel, Seller>().ReverseMap();

            var mapperConfig = new MapperConfiguration(cfg);
            services.AddSingleton<IMapper, Mapper>(x => new Mapper(mapperConfig));

            return services;
        }
    }
}
