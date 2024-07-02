using EntregaTudo.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController(
    IWebHostEnvironment webHostEnvironment,
    ILogger logger,
    IServiceProvider serviceProvider,
    IConfiguration config,
    ICustomerRepository customer)
    : ApiControllerBase(webHostEnvironment, logger, serviceProvider)
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        var person = customer.GetPersonByEmail(login.Email);

        if (person == null)
            return Unauthorized();

        var authenticate = customer.VerifyPassword(login.Password, person.PasswordHash);

        if(!authenticate) return Unauthorized();

        var token = GenerateJwtToken();
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["Jwt:ExpireMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}