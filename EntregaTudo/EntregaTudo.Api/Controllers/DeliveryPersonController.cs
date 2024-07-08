using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Domain.User;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using VehicleStatus = EntregaTudo.Shared.Enums.VehicleStatus;
using VehicleType = EntregaTudo.Shared.Enums.VehicleType;

namespace EntregaTudo.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class DeliveryPersonController(
    IWebHostEnvironment webHostEnvironment,
    ILogger<DeliveryPersonController> logger,
    IServiceProvider serviceProvider,
    IDeliveryPersonRepository repository)
    : RestApiControllerBase<IDeliveryPersonRepository, DeliveryPerson, DeliveryPersonDto>(webHostEnvironment, logger,
        serviceProvider, repository)
{
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
            PhoneNumber = domain.PhoneNumber,
            Password = domain.PasswordHash,
            Vehicle = new VehicleDto
            {
                Id = domain.Id,
                Brand = domain.Vehicle?.Brand,
                LicensePlate = domain.Vehicle?.LicensePlate,
                LoadCapacity = domain.Vehicle?.LoadCapacity,
                ManufactureYear = domain.Vehicle?.ManufactureYear,
                Model = domain.Vehicle?.Model,
                VehicleStatus = (VehicleStatus?)domain.Vehicle?.VehicleStatus,
                VehicleType = (VehicleType?)domain.Vehicle?.VehicleType,
            }
        };
    }

    public override async Task<DeliveryPerson> ToDomainAsync(DeliveryPersonDto dto)
    {

        var vehicleId = dto.Vehicle?.Id ?? ObjectId.GenerateNewId();

        return new DeliveryPerson
        {
            Id = dto.Id ?? ObjectId.Empty,
            FirstName = dto.FirstName,
            DocumentNumber = dto.DocumentNumber,
            Email = dto.Email,
            LastName = dto.LastName,
            PersonType = PersonType.DeliveryPerson,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = Repository.HashPassword(dto.Password),
            Vehicle = new Vehicle
            {
                Id = vehicleId,
                Brand = dto.Vehicle?.Brand,
                LicensePlate = dto.Vehicle?.LicensePlate,
                LoadCapacity = dto.Vehicle?.LoadCapacity,
                ManufactureYear = dto.Vehicle?.ManufactureYear,
                Model = dto.Vehicle?.Model,
                VehicleStatus = Core.Domain.Enum.VehicleStatus.Available,
                VehicleType = (Core.Domain.Enum.VehicleType?)dto.Vehicle?.VehicleType,
            }
        };
    }
}
