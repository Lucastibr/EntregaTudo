using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Core.Domain.Base;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Dal.Context;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using VehicleType = EntregaTudo.Shared.Enums.VehicleType;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class VehicleController : RestApiControllerBase<Vehicle, VehicleDto, EntregaTudoDbContext>
{
    public VehicleController(EntregaTudoDbContext context) : base(context)
    {
    }

    protected override Vehicle ToDomain(VehicleDto dto, Vehicle? entity = null)
    {
        return new Vehicle
        {
            Id = dto.Id,
            Brand = dto.Brand ?? entity?.Brand,
            LicensePlate = dto.LicensePlate ?? entity?.LicensePlate,
            LoadCapacity = dto.LoadCapacity ?? entity?.LoadCapacity,
            ManufactureYear = dto.ManufactureYear ?? entity?.ManufactureYear,
            Model = dto.Model ?? entity?.Model,
            VehicleStatus = (VehicleStatus?)dto.VehicleStatus ?? VehicleStatus.Available,
            VehicleType = (Core.Domain.Enum.VehicleType?)(dto.VehicleType ?? VehicleType.Motorcycle)
        };
    }

    protected override VehicleDto ToDto(Vehicle domain)
    {
        return new VehicleDto
        {
            Id = domain.Id,
            Brand = domain.Brand,
            LicensePlate = domain.LicensePlate,
            LoadCapacity = domain.LoadCapacity,
            ManufactureYear = domain.ManufactureYear,
            Model = domain.Model,
            VehicleStatus = (Shared.Enums.VehicleStatus?)domain.VehicleStatus,
            VehicleType =(VehicleType?)domain.VehicleType
        };
    }
}