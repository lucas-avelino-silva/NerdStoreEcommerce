using SNE.WebApp.MVC.Models;

namespace SNE.WebApp.MVC.Services
{
    public interface IAutentificacaoService
    {
        Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin);

        Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro);
    }

}
