using Microsoft.AspNetCore.Mvc;
using SNE.WebApp.MVC.Models;
using System.Diagnostics;

namespace SNE.WebApp.MVC.Controllers
{
    public class HomeController : Controller
    {
        [Route("sistema-indisponivel")]
        public IActionResult SistemaIndisponivel()
        {
            return View("Error", new ErrorViewModel
            {
                Titulo = "Sistema indisponivel",
                Mensagem = "O sistema está temporariamente indisponivel, isto pode ocorrer em momentos de sobrecarga de usuários.",
                ErroCode = 500
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var modelErro = new ErrorViewModel();

            if(id == 500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";

                modelErro.Titulo = "Ocorreu um erro!";

                modelErro.ErroCode = id;
            }
            else if(id == 404)
            {
                modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";

                modelErro.Titulo = "Ops! Página não encontrada.";

                modelErro.ErroCode = id;
            }
            else if (id == 403)
            {
                modelErro.Mensagem = "Você não tem permissão para fazer isto.";

                modelErro.Titulo = "Acesso Negado";

                modelErro.ErroCode = id;
            }
            else
            {
                return StatusCode(404);
            }

            return View("Error", modelErro);
        }
    }
}