using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Controllers;
using System.Xml.Schema;

namespace NSE.Bff.Compras.Controllers
{
    [Authorize]
    [ApiController]
    public class CarrinhoController : MainController
    {
        private readonly ICarrinhoService _carrinhoService;
        private readonly ICatalogoService _catalogoService;
        private readonly IPedidoService _pedidoService;

        public CarrinhoController(ICatalogoService catalogoService, ICarrinhoService carrinhoService, IPedidoService pedidoService)
        {
            _catalogoService = catalogoService;

            _carrinhoService = carrinhoService;

            _pedidoService = pedidoService;
        }

        [HttpGet("compras/carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _carrinhoService.ObterCarrinho());
        }

        [HttpGet("compras/carrinho-quantidade")]
        public async Task<int?> ObterQuantidadeCarrinho()
        {
            var quanridade = await _carrinhoService.ObterCarrinho();

            var total = quanridade?.Itens.Sum(x => x.Quantidade);

            if(total <= 0 || total == null)
            {
                total = 0;
            }

            return total;
        }

        [HttpPost("compras/carrinho/itens")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

            await ValidarItemCarrinho(produto, itemProduto.Quantidade);

            if (!OperacaoValida()) return CustomResponse();

            itemProduto.Nome = produto.Nome;

            itemProduto.Valor = produto.Valor;

            itemProduto.Imagem = produto.Imagem;

            var resposta = await _carrinhoService.AdicionarItemCarrinho(itemProduto);

            return CustomResponse(resposta);
        }

        [HttpPut("compras/carrinho/itens/{produtoId}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

            await ValidarItemCarrinho(produto, itemProduto.Quantidade);

            if (!OperacaoValida()) return CustomResponse();

            var resposta = await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

            return CustomResponse(resposta);
        }

        [HttpDelete("compras/carrinho/itens/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            if(produto == null)
            {
                AdicionarErroProcessamento("Produto inexistente!");

                return CustomResponse();
            }

            var resposta = await _carrinhoService.RemoverItemCarrinho(produtoId);

            return CustomResponse(resposta);
        }

        [HttpPost("compras/carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
        {
            var voucher = await _pedidoService.ObterVoucherPorCodigo(voucherCodigo);

            if(voucher is null)
            {
                AdicionarErroProcessamento("Voucher inválido ou nãi encontrado.");
                return CustomResponse();
            }

            var resposta = await _carrinhoService.AplicarVoucherCarrinho(voucher);

            return CustomResponse(resposta);
        }

        private async Task ValidarItemCarrinho(ItemProdutoDTO produto, int quantidade)
        {
            if (produto == null) AdicionarErroProcessamento("Produto não existe.");

            if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto.Nome}.");

            var carrinho = await _carrinhoService.ObterCarrinho();

            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto.Id);

            if(itemCarrinho != null && itemCarrinho.Quantidade + quantidade > produto.QuantidadeEstoque)
            {
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, e você selecionou {quantidade}.");
            return;
            }

            if (quantidade > produto.QuantidadeEstoque) AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, e você selecionou {quantidade}.");

        }
    }
}
