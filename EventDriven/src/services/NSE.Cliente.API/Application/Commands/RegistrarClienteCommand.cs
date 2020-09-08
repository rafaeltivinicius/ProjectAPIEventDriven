using System;

namespace NSE.Cliente.API.Application.Commands
{
    // Command representa uma intensão de alteração no estado da entidade (insert, update, delete)
    public class RegistrarClienteCommand
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }
    }
}
