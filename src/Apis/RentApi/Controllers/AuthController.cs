using Application.Commands.Auth;
using Domain.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentApi.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService  _authService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AuthController(IAuthService authService, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _authService = authService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Realiza autenticação de usuários administradores e entregadores.
        /// </summary>
        /// 
        /// <response code="200">Retorna o token de autenticação.</response>
        /// <response code="400">An error occurred while searching for the motorcycle.</response>
        /// <response code="401">Se o os dados de autenticação forem inválidos.</response>
        /// <response code="500">Retorna um código 500 se acontecer algum erro interno no servidor.</response>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginCommand login, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _authService.Authenticate(login.Identity, login.Password, cancellationToken);

                if (result.IsLeft && result.Left.Message != "Not found")
                {
                    return Unauthorized();
                }

                if (result.IsLeft)
                    return Unauthorized();

                var tokenHandler = new JwtSecurityTokenHandler();

                var key = _configuration["Jwt:Key"];

                if (string.IsNullOrEmpty(key))
                {
                    _logger.LogError("The JWT key cannot be null or empty. Please check the environment settings");
                    throw new InvalidOperationException("JWT is null");
                }

                var keyBytes = Encoding.UTF8.GetBytes(key);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, result.Right.Id.ToString()),
                        new Claim("identity", result.Right.Identity),
                        new Claim(ClaimTypes.Role, result.Right.Role.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(48),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new { Token = tokenHandler.WriteToken(token) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating token");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Internal server error" });
            }
        }
    }
}
