using Microsoft.Extensions.Options;
using NSE.WebAPI.Core.Usuario;
using SNE.Core.Communication;
using SNE.WebApp.MVC.Extensions;
using SNE.WebApp.MVC.Models;

namespace SNE.WebApp.MVC.Services
{
    public class ComprasBffService : Service, IComprasBffService
    {
        private HttpClient _httpClient;

        public ComprasBffService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl);
        }

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var result = await _httpClient.GetAsync("/compras/carrinho/");

            TratarErrosResponse(result);

            return await DeserializarObjetoResponse<CarrinhoViewModel>(result); 
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PostAsync("/compras/carrinho/itens/", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PutAsync($"/compras/carrinho/itens/{ produto.ProdutoId}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"/compras/carrinho/itens/{ produtoId}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        public async Task<int> ObterQuantidadeCarrinho()
        {
            var response = await _httpClient.GetAsync("/compras/carrinho-quantidade");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<int>(response);
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
        {
            var itemContent = ObterConteudo(voucher);

            var response = await _httpClient.PostAsync("/compras/carrinho/aplicar-voucher", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }
    }
}
