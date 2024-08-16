using EntregaTudo.Api.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EntregaTudo.Core.Domain.Enum;
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
    ICustomerRepository customer,
    IDeliveryPersonRepository deliveryPerson)
    : ApiControllerBase(webHostEnvironment, logger, serviceProvider)
{

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
            PersonType = user.FindFirst("PersonType")?.Value,
            Id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
        };

        return Ok(userInfo);

    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto login)
    {
        Person? person;

        person = customer.GetPersonByEmail(login.Email) ?? (Person?)deliveryPerson.GetDeliveryPersonByEmail(login.Email);

        var authenticate = person.PersonType == PersonType.User
            ? customer.VerifyPassword(login.Password, person.PasswordHash)
            : deliveryPerson.VerifyPassword(login.Password, person.PasswordHash);

        if (!authenticate)
            return Unauthorized();
        
        var token = GenerateJwtToken(person);

        var personType = person.PersonType.ToString();

        return Ok(new { Token = token, CustomerId = person.Id.ToString(), userName = person.FirstName, userType = personType });
    }

    private string GenerateJwtToken(dynamic? person)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()),
            new Claim("FirstName", person.FirstName),
            new Claim("LastName", person.LastName),
            new Claim(ClaimTypes.Email, person.Email),
            new Claim("DocumentNumber", person.DocumentNumber),
            new Claim("PhoneNumber", person.PhoneNumber),
            new Claim("PersonType", person.PersonType.ToString())
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