using Microsoft.AspNetCore.Mvc;
using SNE.WebApp.MVC.Models;
using SNE.WebApp.MVC.Services;

namespace SNE.WebApp.MVC.Extensions
{
    public class CarrinhoViewComponent : ViewComponent
    {
        private readonly IComprasBffService _comprasBffService;

        public CarrinhoViewComponent(IComprasBffService comprasBffService)
        {
            _comprasBffService = comprasBffService;
        }

        //criar a view dele na pasta Components, é necessario criar outra pasta dentro de Components com o nome "Carrinho"
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _comprasBffService.ObterQuantidadeCarrinho());
        }
    }
}
