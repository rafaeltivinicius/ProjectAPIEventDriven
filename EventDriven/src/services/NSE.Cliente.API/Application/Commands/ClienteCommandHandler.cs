using FluentValidation.Results;
using MediatR;
using NSE.Cliente.API.Models;
using NSE.Core.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Cliente.API.Application.Commands
{
    //Manipulador do commando
    public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido())
                return message.ValidationResult;

            var cliente = new Models.Cliente(message.Id, message.Nome, message.Email, message.Cpf);

            //Validações de negocio
            var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

            //Persisti no banco
            if (clienteExistente != null)// ja existem o cliente
            {
                AdicionarErro("Este CPF já existe");
                return ValidationResult;
            }

            _clienteRepository.Adicionar(cliente);

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }

    }
}
