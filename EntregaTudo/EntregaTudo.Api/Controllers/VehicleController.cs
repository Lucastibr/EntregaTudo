using Codout.Framework.DAL;
using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Repository;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using VehicleType = EntregaTudo.Shared.Enums.VehicleType;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController(
    IWebHostEnvironment webHostEnvironment,
    ILogger<CustomerController> logger,
    IUnitOfWork unitOfWork,
    IServiceProvider serviceProvider,
    IVehicleRepository repository)
    : RestApiControllerBase<IVehicleRepository, Vehicle, VehicleDto>(webHostEnvironment, logger, unitOfWork,
        serviceProvider, repository)
{
    public override async Task<VehicleDto> ToDtoAsync(Vehicle value)
    {
        return new VehicleDto
        {
            Id = value.Id,
            Brand = value.Brand,
            LicensePlate = value.LicensePlate,
            LoadCapacity = value.LoadCapacity,
            ManufactureYear = value.ManufactureYear,
            Model = value.Model,
            VehicleStatus = (Shared.Enums.VehicleStatus?)value.VehicleStatus,
            VehicleType = (VehicleType?)value.VehicleType
        };
    }

    public override async Task<Vehicle> ToDomainAsync(VehicleDto dto)
    {
        return new Vehicle
        {
            Id = dto.Id.Value,
            Brand = dto.Brand,
            LicensePlate = dto.LicensePlate,
            LoadCapacity = dto.LoadCapacity,
            ManufactureYear = dto.ManufactureYear,
            Model = dto.Model,
            VehicleStatus = (VehicleStatus?)dto.VehicleStatus ?? VehicleStatus.Available,
            VehicleType = (Core.Domain.Enum.VehicleType?)(dto.VehicleType ?? VehicleType.Motorcycle)
        };
    }
}