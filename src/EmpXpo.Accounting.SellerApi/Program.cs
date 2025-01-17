
using EmpXpo.Accounting.CashFlowSellerApi.Extensions;
using EmpXpo.Accounting.CashFlowSellerApi.Filters;
using EmpXpo.Accounting.CashFlowSellerApi.Middlewares;
using EmpXpo.Accounting.Ioc;
using Microsoft.OpenApi.Models;

namespace EmpXpo.Accounting.CashFlowSellerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                   .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .AddCommandLine(args);

            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(options =>
                            {
                                options.SwaggerDoc("v1", new OpenApiInfo
                                {
                                    Title = "Swagger Documentação Web API",
                                    Description = "Api with Seller's CRUD operations",
                                    Contact = new OpenApiContact() { Name = "Felipe Jonh Doe", Email = "Felipejonhdoe@email.com.br" },
                                    License = new OpenApiLicense()
                                    {
                                        Name = "MIT License",
                                        Url = new Uri("https://opensource.org/licenses/MIT")
                                    }
                                });
                                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "EmpXpo.Accounting.SellerApi.xml"));
                            })
                            .AddWebApiSeller()
                            .AddContainerIoc(builder.Configuration)
                            .AddControllers(options =>
                            {
                                options.Filters.Add<NotificationFilter>();
                            })
                            .AddJsonOptions(options =>
                                            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                                           );

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<SellerExceptionHandlingMiddleware>()
               .UseHttpsRedirection()
               .UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
