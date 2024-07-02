using Codout.Framework.DAL;
using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EntregaTudo.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class CustomerController(
    IWebHostEnvironment webHostEnvironment,
    ILogger<CustomerController> logger,
    IServiceProvider serviceProvider,
    ICustomerRepository repository)
    : RestApiControllerBase<ICustomerRepository, Customer, CustomerDto>(webHostEnvironment, logger, serviceProvider,
        repository)
{
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
            PhoneNumber = domain.PhoneNumber,
            Password = domain.PasswordHash
        };
    }

    public override async Task<Customer> ToDomainAsync(CustomerDto dto)
    {
        return new Customer
        {
            Id = dto.Id ?? ObjectId.Empty,
            FirstName = dto.FirstName,
            DocumentNumber = dto.DocumentNumber,
            Email = dto.Email,
            LastName = dto.LastName,
            PersonType = (PersonType?)dto.PersonType ?? PersonType.User,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = Repository.HashPassword(dto.Password)
        };
    }
}
