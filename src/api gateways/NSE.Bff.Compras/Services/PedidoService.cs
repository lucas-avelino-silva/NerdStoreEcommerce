using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;
using NSE.Bff.Compras.Models;
using System.Net;

namespace NSE.Bff.Compras.Services
{
    public interface IPedidoService
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }

    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _HttpClient;

        public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _HttpClient = httpClient;

            _HttpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var response = await _HttpClient.GetAsync($"/Voucher/voucher/{codigo}/");

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<VoucherDTO>(response);
        }
    }
}
