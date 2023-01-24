using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNE.Core.Messages
{
    //Para o mediatr entender q ele é um command utilizamos a interface IRequest
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public Command()
        {
            Timestamp= DateTime.Now;
        }

        public DateTime Timestamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public virtual bool Valido()
        {
            throw new NotImplementedException();
        }
    }
}
