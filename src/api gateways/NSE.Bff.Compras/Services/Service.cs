using System.Text.Json;
using System.Text;
using System.Net;

namespace NSE.Bff.Compras.Services
{
    public abstract class Service
    {
        protected StringContent ObterConteudo(object dado)
        {
            // vai retornar um dado no formato string, mas vou tratar ele em um formto especifico
            // media type(3° parametro) é o tipo que colocamos no headller informando o tipo de dados que estamos passando, ou seja, eu estou convertendo para tipo string, mas ele está formatado em formato json.

            var ContentSerealizado = new StringContent(
                JsonSerializer.Serialize(dado),
                Encoding.UTF8,
                "application/json");

            return ContentSerealizado;
        }

        protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            //esse cara é pra eu consegui desentralisar para o objeto abaixo, estava dando erro pq o nome da propriedade estava com letra maiuscula e o retorno da api vem td minusculo
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }

        protected bool TratarErrosResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest) return false;

            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
