using Microsoft.Extensions.Options;
using SNE.WebApp.MVC.Extensions;
using SNE.WebApp.MVC.Models;
using System.Text.Json;

namespace SNE.WebApp.MVC.Services
{
    public class AutentificacaoService : Service, IAutentificacaoService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettings _settings;

        public AutentificacaoService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl);
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
        {
            // vai retornar um dado no formato string, mas vou tratar ele em um formto especifico
            // media type(3° parametro) é o tipo que colocamos no headller informando o tipo de dados que estamos passando, ou seja, eu estou convertendo para tipo string, mas ele está formatado em formato json.
            var loginContent = ObterConteudo(usuarioLogin);

                       //vai concatenar com o BaseAddress  do httpClient configurado no construtor
            var response = await _httpClient.PostAsync("/api/identidade/autenticar", loginContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                                                                    //classe de erros
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }

        public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
        {
            var registroContent = ObterConteudo(usuarioRegistro);

                                                         //outra forma de fazer, menos indicado
            var response = await _httpClient.PostAsync($"{_settings.AutenticacaoUrl}/api/identidade/nova-conta", registroContent);

            if (!TratarErrosResponse(response))
            {
                return new UsuarioRespostaLogin
                {
                    ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
                };
            }

            return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
        }
    }
}
