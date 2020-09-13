using EasyNetQ;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSE.Clientes.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;
using NSE.MessageBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Services
{
    // manipula integração e funciona em Background
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider; //a interface da Startup.cs onde regisra os serviços

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {
            //aqui ele recebe o serviço que vem da API de Indentidade - RegistrarCliente (fica ouvindo a fila
            // ele escuta o tempo inteiro
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarCliente(request));

            //esse é evento é disparado quando a fila esta conectada a aplicação
            _bus.AdvancedBus.Connected += OnConnect;
        }

        //só chama esse metodo 1°x que é ao iniciar a aplicação
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        //abstração para chamar o Mediator que chama ClienteCommandHandler.cs
        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            ValidationResult sucesso;
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);

            // o escopo q estou trabalhando é singleton
            using (var scope = _serviceProvider.CreateScope())//pego o container de Inj Dep, crio um scopo e busco ele com base na interface
            {   //por BackgroundService trabalhar com singleton, não posso chamar uma instancia 
                // de IMediatorHandler (pois é scoped), e scoped não comunica com singleton
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>(); //service locate 

                sucesso = await mediator.EnviarComando(clienteCommand);
            }

            return new ResponseMessage(sucesso);
        }
    }
}
