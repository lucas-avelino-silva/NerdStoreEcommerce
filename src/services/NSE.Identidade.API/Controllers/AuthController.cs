using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Models;
using NSE.MessageBus;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identidade;
using SNE.Core.Messages.Integration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identidade")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IMessageBus _bus;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings, IMessageBus bus)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _bus = bus;
        }

        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                usuarioRegistro.Cpf = usuarioRegistro.Cpf.Replace(".", string.Empty).Replace("-", string.Empty);
                // Integrando com a api de cliente
                var clienteResult = await RegistrarCliente(usuarioRegistro);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);

                    return CustomResponse(clienteResult.ValidationResult);
                }

                //await _signInManager.SignInAsync(user, false);
                return CustomResponse(await GerarJwt(usuarioRegistro.Email));
            }

            foreach(var erro in result.Errors)
            {
                AdicionarErroProcessamento(erro.Description);
            }

            return CustomResponse();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioRegistro.Email);

            var usuarioRegistrado = new UsuarioRegistradoIntegrationEvent(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

            try
            {
                //Tipo de dado q estou passsando e o tipo de dado q espero como resposta
                //metodo q criei no core.
                var sucesso = await _bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
                
                return sucesso;
            }
            catch
            {
                await _userManager.DeleteAsync(usuario);

                throw;
            }
        }


        [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await GerarJwt(usuarioLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuário ou Senha inválidas");

            return CustomResponse();
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = await _userManager.GetClaimsAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            //sao os claim do token
            //Sub é o nosso usuario. Jti é o id do token. os dois ultimos é quando o token foi emitido. as Roles são consideradas claim.
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            //data de expiração e emissao
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));


            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours((double)_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identityClaims
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours((double)_appSettings.ExpiracaoHoras).TotalSeconds.ToString(),
                UserToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(x => new UsuarioClaim { Type = x.Type, Value = x.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
