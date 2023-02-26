using Microsoft.Extensions.Options;
using NSE.Bff.Compras.Extensions;

namespace NSE.Bff.Compras.Services
{
    public interface IPagamentoService
    {

    }

    public class PagamentoService : Service, IPagamentoService
    {
        private readonly HttpClient _HttpClient;

        public PagamentoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _HttpClient = httpClient;

            _HttpClient.BaseAddress = new Uri(settings.Value.PagamentoUrl);
        }
    }
}
