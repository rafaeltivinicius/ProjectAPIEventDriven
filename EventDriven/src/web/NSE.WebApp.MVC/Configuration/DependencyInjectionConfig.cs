﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace NSE.WebApp.MVC.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider,CpfValidationAttributeAdapterProvider>();

            //Registro Delegate
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            services.AddHttpClient<ICatalogoService, CatalogoService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()//Delegate - Intercepta qualquer request q vem Desse serviço
                //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(1))); //policy
                .AddPolicyHandler(PollyExtensions.EsperarTentar()) //Policy de tentativa caso ocorra falhas de rede 
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));// o CircuitBreaker conta a requisição como um todo da aplicação (nao é por usuario) 

            #region example Refit

            // Trabalhando com Refit no CatalogoService

            //services.AddHttpClient("Refit", options =>         //cofigura BaseAdress e IOptions<AppSettings> settings
            //{
            //    options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
            //})
            //.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            //{
            //    ClientCertificateOptions = ClientCertificateOption.Manual, //ignorando o certificado da maquina
            //    ServerCertificateCustomValidationCallback =
            //        (httpRequestMessage, cert, cetChain, policyErrors) =>
            //        {
            //            return true;
            //        }
            //})
            //.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() //Intercepta qualquer request q vem desse serviço
            //.AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>); //  cria o typeClient em tempo de execução

            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();
        }
    }

    //criando minha policy - Retry patterns
    public class PollyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            var retry = HttpPolicyExtensions
                .HandleTransientHttpError() //codições de falhas de rede
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                }, (outcome, timespan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Tentando pela {retryCount} vez!");
                    Console.ForegroundColor = ConsoleColor.White;
                });

            return retry;
        }
    }
}
