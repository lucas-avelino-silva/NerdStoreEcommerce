using SNE.Core.Communication;
using SNE.WebApp.MVC.Models;

namespace SNE.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        Task<CarrinhoViewModel> ObterCarrinho();

        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto);

        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);

        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);

        Task<int> ObterQuantidadeCarrinho();

        Task<ResponseResult> AplicarVoucherCarrinho(string voucher);
    }
}
