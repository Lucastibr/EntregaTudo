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
public class DeliveryPersonController : RestApiControllerBase<IDeliveryPersonRepository, DeliveryPerson, DeliveryPersonDto>
{
    public DeliveryPersonController(IWebHostEnvironment webHostEnvironment,
        ILogger<DeliveryPersonController> logger,
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider,
        IDeliveryPersonRepository repository)
        : base(webHostEnvironment, logger, unitOfWork, serviceProvider, repository)
    {
    }

    public override async Task<DeliveryPersonDto> ToDtoAsync(DeliveryPerson domain)
    {
        return new DeliveryPersonDto
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

    public override async Task<DeliveryPerson> ToDomainAsync(DeliveryPersonDto dto)
    {
        return new DeliveryPerson
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
