using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SNE.WebApp.MVC.Models;
using SNE.WebApp.MVC.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SNE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {
        private readonly IAutentificacaoService _autentificacaoService;

        public IdentidadeController(IAutentificacaoService autentificacaoService)
        {
            _autentificacaoService = autentificacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            // API registro
            var result = await _autentificacaoService.Registro(usuarioRegistro);

            if (ResponsePossuiErros(result.ResponseResult)) return View(usuarioRegistro);

            //realizar o login
            await RealizarLogin(result);

            return RedirectToAction("index", "home");
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(usuarioLogin);

            // API login
            var result = await _autentificacaoService.Login(usuarioLogin);

            if (ResponsePossuiErros(result.ResponseResult)) return View(usuarioLogin);

            //realizar o login
            await RealizarLogin(result);

            if(string.IsNullOrEmpty(returnUrl)) return RedirectToAction("index", "home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [Route("sair")]
        public async Task<IActionResult> Sair()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "home");
        }

        private async Task RealizarLogin(UsuarioRespostaLogin resposta)
        {
            var token = ObterTokenFormatado(resposta.AccessToken);

            var claims = new List<Claim>();

            claims.Add(new Claim("JWT", resposta.AccessToken));

            claims.AddRange(token.Claims);

            //vai conseguir gerar minahs claims dentro do cookie
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //configurando o cookie, quando q vai expirar e se ele é persistente, é persistente pq ele nao vai durar apenas um request, ele vai durar em muitos request dentro do periodo de validade
            var authPropriedades = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            //vou passar um schema q é o nome q representa q tipo de autentificacao eu estou usando - vou trabalhar com esquema de autentificacao com cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authPropriedades);
        }

        private static JwtSecurityToken ObterTokenFormatado(string jwtToken)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(jwtToken) as JwtSecurityToken;
        }
    }
}
