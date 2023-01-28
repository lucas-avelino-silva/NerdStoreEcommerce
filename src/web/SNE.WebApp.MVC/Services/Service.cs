using SNE.WebApp.MVC.Extensions;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;
using SNE.WebApp.MVC.Models;

namespace SNE.WebApp.MVC.Services
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
            switch ((int)response.StatusCode)
            {
                case 401:
                case 403:
                case 404:
                case 500:
                    throw new CustomHttpRequestException(response.StatusCode);

                case 400:
                    return false;
            }

            //aqui ele garante q foi sucesso. ex: se passar foi sucesso, se ele parar nesta linha ele vai estourar um exception
            //é uma forma de garantir q por mais q eu nao tenha previsto ele valide.
            response.EnsureSuccessStatusCode();

            return true;
        }
    }
}
