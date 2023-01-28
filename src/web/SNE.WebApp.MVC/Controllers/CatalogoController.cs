using Microsoft.AspNetCore.Mvc;
using NSE.WebAPI.Core.Controllers;
using SNE.WebApp.MVC.Services;

namespace SNE.WebApp.MVC.Controllers
{
    [Route("Catalogo")]
    public class CatalogoController : MainController
    {
        private readonly ICatalogoService _CatalogoService;
        private readonly ICatalogoServiceRefit _CatalogoServiceRefit;
        public CatalogoController(ICatalogoService CatalogoService, ICatalogoServiceRefit catalogoServiceRefit)
        {
            _CatalogoService = CatalogoService;
            _CatalogoServiceRefit = catalogoServiceRefit;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            //var produtos = await _CatalogoService.ObterTodos();
            var produtos = await _CatalogoService.ObterTodos();

            return View(produtos);
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            //var produto = await _CatalogoService.ObterPorId(id);
            var produto = await _CatalogoService.ObterPorId(id);

            return View(produto);
        }
    }
}
