using EmpXpo.Accounting.Application;
using EmpXpo.Accounting.CashFlowApi.Extensions;
using EmpXpo.Accounting.CashFlowApi.Filters;
using EmpXpo.Accounting.CashFlowApi.Middlewares;
using EmpXpo.Accounting.Ioc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Models;

namespace EmpXpo.Accounting.CashFlowApi
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
                            .AddSwaggerGen(options => {
                                options.SwaggerDoc("v1", new OpenApiInfo
                                {
                                    Title = "Swagger Documentação Web API",
                                    Description = "API for credit and debit entries",
                                    Contact = new OpenApiContact() { Name = "Felipe Jonh Doe", Email = "Felipejonhdoe@email.com.br" },
                                    License = new OpenApiLicense()
                                    {
                                        Name = "MIT License",
                                        Url = new Uri("https://opensource.org/licenses/MIT")
                                    }
                                });
                                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "EmpXpo.Accounting.CashFlowApi.xml"));
                            })
                            .AddWebApiCashFlow()
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

            app.UseMiddleware<CashFlowExceptionHandlingMiddleware>()
               .UseHttpsRedirection()
               .UseAuthorization();            
            
            
            app.MapControllers();

            app.Run();
        }
    }
}
