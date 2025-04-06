using ApiServer.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLogin userLogin, [FromServices] JwtSettings jwt)
    {
        // Validar as credenciais do usuário (substitua por sua lógica de validação)
        if (userLogin.Username == "usuario" && userLogin.Password == "senha")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwt.SecretKey);

            var tokenExpires = DateTime.Now.AddMinutes(Random.Shared.Next(1, jwt.ExpirationMinutes));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userLogin.Username)
                }),
                Expires = tokenExpires.ToUniversalTime(),
                Audience = jwt.Audience,
                Issuer = jwt.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = tokenString, ExpiresIn = tokenExpires });
        }

        return Unauthorized();
    }
}

public class UserLogin
{
    public string Username { get; set; }
    public string Password { get; set; }
}
