using APICatalogo.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APICatalogo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly IConfiguration _configuration;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signManager = signManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Define se a api está acessível
        /// </summary>
        /// <returns>Retorna uma mensagem com a data de acesso</returns>
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acessado em :" + DateTime.Now.ToLongDateString();
        }

        /// <summary>
        /// Registra um usuário para acesso a aplicação
        /// </summary>
        /// <param name="usuarioDTO">Recebe um objeto do tipo UsuarioDTO</param>
        /// <returns>Status 200 e o token para acesso</returns>
        /// <remarks>retorna o status 200 e o token para o novo usuário</remarks>
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] UsuarioDTO usuarioDTO)
        {
            var user = new IdentityUser
            {
                UserName = usuarioDTO.Email,
                Email = usuarioDTO.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _signManager.SignInAsync(user, false);
            return Ok(GeraToken(usuarioDTO));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UsuarioDTO usuarioDTO)
        {
            var result = await _signManager.PasswordSignInAsync(usuarioDTO.Email,
                usuarioDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Login Inválido...");
                return BadRequest(ModelState);

            }

            return Ok(GeraToken(usuarioDTO));
        }

        private UsuarioTokenDTO GeraToken(UsuarioDTO usuarioDTO)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, usuarioDTO.Email),
                new Claim("meuTest", "sharp"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expire = _configuration["TokenConfiguration:ExpireHours"];
            var expiration = DateTime.UtcNow.AddHours(Convert.ToDouble(expire));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new UsuarioTokenDTO()
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = "Token JWT Ok"
            };
        }

    }
}
