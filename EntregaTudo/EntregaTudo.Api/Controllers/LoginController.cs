using EntregaTudo.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared;
using Microsoft.AspNetCore.Authorization;

namespace EntregaTudo.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class LoginController(
    IWebHostEnvironment webHostEnvironment,
    ILogger<LoginController> logger,
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

        if (!authenticate) return Unauthorized();

        var token = GenerateJwtToken(person);
        return Ok(new { Token = token });
    }

    [HttpGet("userinfo")]
    [Authorize]
    public IActionResult GetUserInfo()
    {
        if (User.Identity is not ClaimsIdentity user) return BadRequest("User not found");

        var userInfo = new
        {
            FirstName = user.FindFirst("FirstName")?.Value,
            LastName = user.FindFirst("LastName")?.Value,
            Email = user.FindFirst(ClaimTypes.Email)?.Value,
            DocumentNumber = user.FindFirst("DocumentNumber")?.Value,
            PhoneNumber = user.FindFirst("PhoneNumber")?.Value,
            PersonType = user.FindFirst("PersonType")?.Value
        };

        return Ok(userInfo);

    }

    private string GenerateJwtToken(Customer user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("DocumentNumber", user.DocumentNumber),
            new Claim("PhoneNumber", user.PhoneNumber),
            new Claim("PersonType", user.PersonType.ToString())
        };

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["Jwt:ExpireMinutes"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}