﻿using FluentValidation.Results;
using MediatR;
using System;

namespace NSE.Core.Messages
{
    //alterão estado de uma entidade, representa uma intensão de alteração de estatus
    public abstract class Command : Message, IRequest<ValidationResult> //interface de marcação
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; } //classe para objetos de validação

        public Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
