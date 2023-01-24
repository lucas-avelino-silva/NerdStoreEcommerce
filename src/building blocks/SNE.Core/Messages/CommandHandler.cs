using FluentValidation.Results;
using SNE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNE.Core.Messages
{
    public abstract class CommandHandler
    {
        public CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected ValidationResult ValidationResult;

        protected void AdicionarErro(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistirDados(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AdicionarErro("Houve um erro ao persistir os dados.");

            return ValidationResult;
        }
    }
}
