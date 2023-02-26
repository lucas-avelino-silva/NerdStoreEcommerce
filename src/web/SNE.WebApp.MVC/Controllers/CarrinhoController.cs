using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Controllers;
using SNE.WebApp.MVC.Models;
using SNE.WebApp.MVC.Services;

namespace SNE.WebApp.MVC.Controllers
{
    [Authorize]
    [Route("Carrinho")]
    public class CarrinhoController : MainController
    {
        private readonly IComprasBffService _comprasBffService;

        public CarrinhoController(IComprasBffService ComprasBffService)
        {
            _comprasBffService = ComprasBffService;

        }

        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _comprasBffService.ObterCarrinho());
        }

        [HttpPost("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemProduto)
        {
            var response = await _comprasBffService.AdicionarItemCarrinho(itemProduto);

            if (ResponsePossuiErros(response)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost("AtualizarItemCarrinho")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var item = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };

            var response = await _comprasBffService.AtualizarItemCarrinho(produtoId, item);

            if (ResponsePossuiErros(response)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        [HttpPost("RemoverItemCarrinho")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _comprasBffService.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(response)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var resposta = await _comprasBffService.AplicarVoucherCarrinho(voucherCodigo);

            if (ResponsePossuiErros(resposta)) return View("Index", await _comprasBffService.ObterCarrinho());

            return RedirectToAction("Index");
        }

        //private void ValidarItemCarrinho(ProdutoViewModel produto, int quantidade)
        //{
        //    if (produto == null) AdicionarErroValidacao("Produto não existe.");
        //    if (quantidade < 1) AdicionarErroValidacao($"Escolha ao menos uma unidade do produto {produto.Nome}.");
        //    if (quantidade > produto.QuantidadeEstoque) AdicionarErroValidacao($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, e você selecionou {quantidade}.");
        //}
    }
}
