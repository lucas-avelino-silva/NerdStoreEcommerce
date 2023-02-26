using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.WebAPI.Core.Controllers;
using SNE.Core.Mediator;

namespace NSE.Clientes.API.Controllers
{
    [ApiController]
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;

        public ClientesController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var obj = new RegistrarClienteCommand(Guid.NewGuid(), "Felipe", "felipe@hotmail.com", "04940911004");
            var result = await _mediatorHandler.EnviarComando(obj);

            return CustomResponse(result);
        }
    }
}
