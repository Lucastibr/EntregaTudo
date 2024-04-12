using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Dal.Context;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : RestApiControllerBase<Person, PersonDto, EntregaTudoDbContext>
{
    public PersonController(EntregaTudoDbContext context) : base(context)
    {
    }

    protected override Person ToDomain(PersonDto dto, Person? person = null)
    {
        return new Person
        {
            Id = dto.Id,
            FirstName = dto.FirstName ?? person?.FirstName,
            DocumentNumber = dto.DocumentNumber ?? person?.DocumentNumber,
            Email = dto.Email ?? person?.Email,
            LastName = dto.LastName ?? person?.LastName,
            PersonType = (PersonType?)dto.PersonType ?? PersonType.User,
            PhoneNumber = dto.PhoneNumber ?? person?.PhoneNumber
        };
    }

    protected override PersonDto ToDto(Person domain)
    {
        return new PersonDto
        {
            Id = domain.Id,
            FirstName = domain.FirstName,
            DocumentNumber = domain.DocumentNumber,
            Email = domain.Email,
            LastName = domain.LastName,
            PersonType = (Shared.Enums.PersonType)domain.PersonType,
            PhoneNumber = domain.PhoneNumber  
        };
    }
}
