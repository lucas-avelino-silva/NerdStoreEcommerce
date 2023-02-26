using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Model;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Carrinho.API.Controllers
{
    [Authorize]
    [ApiController]
    public class CarrinhoController : MainController
    {
        private readonly IAspNetUser _AspNetUser;

        private readonly CarrinhoContext _context;

        public CarrinhoController(IAspNetUser aspNetUser, CarrinhoContext context)
        {
            _AspNetUser = aspNetUser;

            _context = context;
        }

        [HttpGet("carrinho")]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {
            //caso o resultado seja nulo, ele retorna uma instancia de carrinho
            return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
        }

        [HttpPost("adicionarItemcarrinho")]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();

            if(carrinho == null)
            {
                ManipularNovoCarrinho(item);
            }
            else
            {
                ManipularCarrinhoExistente(carrinho, item);
            }

            if (!OperacaoValida()) return CustomResponse();

            var result = await _context.SaveChangesAsync();

            if (result <= 0) AdicionarErroProcessamento("Não foi possivel persistir os dados no banco");

            return CustomResponse();
        }


        [HttpPut("carrinho/{produtoid}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoid, CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();

            var itemCarrinho = await ObterItemCarrinhoValidado(produtoid, carrinho, item);

            if (itemCarrinho == null) return CustomResponse();

            carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

            ValidarCarrinho(carrinho);

            if (!OperacaoValida()) return CustomResponse();

            _context.CarrinhoItens.Update(itemCarrinho);

            _context.CarrinhoCliente.Update(carrinho);

            var result = await _context.SaveChangesAsync();

            if (result <= 0) AdicionarErroProcessamento("Não foi possivel persistir os dados no banco.");

            return CustomResponse();
        }

        [HttpDelete("carrinho/{produtoid}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoid)
        {
            var carrinho = await ObterCarrinhoCliente();

            var itemCarrinho = await ObterItemCarrinhoValidado(produtoid, carrinho);

            if (itemCarrinho == null) return CustomResponse();

            ValidarCarrinho(carrinho);

            if (!OperacaoValida()) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            _context.CarrinhoItens.Remove(itemCarrinho);

            _context.CarrinhoCliente.Update(carrinho);

            var result = await _context.SaveChangesAsync();

            if (result <= 0) AdicionarErroProcessamento("Não foi possivel persistir os dados no banco.");

            return CustomResponse();
        }

        [HttpPost("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(Voucher voucher)
        {
            var carrinho = await ObterCarrinhoCliente();

            carrinho.AplicarVoucher(voucher);

            _context.CarrinhoCliente.Update(carrinho);

            var result = await _context.SaveChangesAsync();

            if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");

            return CustomResponse();
        }

        private async Task<CarrinhoCliente> ObterCarrinhoCliente()
        {
            return await _context.CarrinhoCliente.Include(x => x.Itens).FirstOrDefaultAsync(x => x.ClienteId == _AspNetUser.ObterUserid());
        }

        private void ManipularNovoCarrinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_AspNetUser.ObterUserid());

            carrinho.AdicionarItem(item);

            ValidarCarrinho(carrinho);

            _context.CarrinhoCliente.Add(carrinho);
        }

        private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var produtoItemExistente = carrinho.CarrinhoItemExistente(item);

            carrinho.AdicionarItem(item);

            ValidarCarrinho(carrinho);

            if (produtoItemExistente)
            {
                _context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId));
            }
            else
            {
                _context.CarrinhoItens.Add(item);
            }

            _context.CarrinhoCliente.Update(carrinho);
        }

        private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
        {
            if(item != null && produtoId != item.ProdutoId)
            {
                AdicionarErroProcessamento("O item não corresponde ao informado.");
                
                return null;
            }

            if(carrinho == null)
            {
                AdicionarErroProcessamento("Carrinho não encontrado");

                return null;
            }

            var itemCarrinho = await _context.CarrinhoItens.FirstOrDefaultAsync(x => x.CarrinhoId == carrinho.Id && x.ProdutoId == produtoId);

            if(itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
            {
                AdicionarErroProcessamento("O item não está no carrinho.");

                return null;
            }

            return itemCarrinho;
        }

        private bool ValidarCarrinho(CarrinhoCliente carrinho)
        {
            if (carrinho.EhValido()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(x => AdicionarErroProcessamento(x.ErrorMessage));

            return false;
        }
    }
}
