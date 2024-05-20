using Codout.Framework.DAL;
using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : RestApiControllerBase<IPersonRepository, Person, PersonDto>
{
    public PersonController(IWebHostEnvironment webHostEnvironment,
        ILogger<PersonController> logger,
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider,
        IPersonRepository repository)
        : base(webHostEnvironment, logger, unitOfWork, serviceProvider, repository)
    {
    }

    public override async Task<PersonDto> ToDtoAsync(Person domain)
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

    public override async Task<Person> ToDomainAsync(PersonDto dto)
    {
        return new Person
        {
            Id = dto.Id.Value,
            FirstName = dto.FirstName,
            DocumentNumber = dto.DocumentNumber,
            Email = dto.Email,
            LastName = dto.LastName,
            PersonType = (PersonType?)dto.PersonType ?? PersonType.User,
            PhoneNumber = dto.PhoneNumber
        };
    }
}
