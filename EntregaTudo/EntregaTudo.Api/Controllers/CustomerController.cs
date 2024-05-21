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
public class CustomerController : RestApiControllerBase<ICustomerRepository, Customer, CustomerDto>
{
    public CustomerController(IWebHostEnvironment webHostEnvironment,
        ILogger<CustomerController> logger,
        IUnitOfWork unitOfWork,
        IServiceProvider serviceProvider,
        ICustomerRepository repository)
        : base(webHostEnvironment, logger, unitOfWork, serviceProvider, repository)
    {
    }

    public override async Task<CustomerDto> ToDtoAsync(Customer domain)
    {
        return new CustomerDto
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

    public override async Task<Customer> ToDomainAsync(CustomerDto dto)
    {
        return new Customer
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
