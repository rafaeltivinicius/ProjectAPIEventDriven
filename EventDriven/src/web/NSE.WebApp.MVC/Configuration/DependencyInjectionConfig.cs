using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using System;
using System.Net.Http;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Registro Delegate
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>()
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual, //ignorando o certificado da maquina
                    ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) =>
                        {
                            return true;
                        }
                });

            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            //    {
            //        ClientCertificateOptions = ClientCertificateOption.Manual, //ignorando o certificado da maquina
            //        ServerCertificateCustomValidationCallback =
            //            (httpRequestMessage, cert, cetChain, policyErrors) =>
            //            {
            //                return true;
            //            }
            //    })//Intercepta qualquer request q vem desse serviço
            //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();


            // Trabalhando com Refit no CatalogoService

            services.AddHttpClient("Refit", options =>         //cofigura BaseAdress e IOptions<AppSettings> settings
            {
                options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual, //ignorando o certificado da maquina
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    }
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Intercepta qualquer request q vem desse serviço
            .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>); //  cria o typeClient em tempo de execução

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
        }
    }
}
