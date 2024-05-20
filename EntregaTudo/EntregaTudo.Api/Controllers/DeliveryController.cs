﻿using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Api.Helpers;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Infrastructure;
using EntregaTudo.Dal.Context;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Repository;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController : ApiControllerBase
{
    private readonly IDeliveryRepository _deliveryRepository;
    public DeliveryController(EntregaTudoDbContext context, IWebHostEnvironment hostingEnvironment, IDeliveryRepository deliveryRepository) : base(context, hostingEnvironment)
    {
        _deliveryRepository = deliveryRepository;
    }

    /// <summary>
    /// Método para calcular o preço do delivery
    /// </summary>
    /// <param name="deliveryDto"></param>
    /// <returns></returns>
    [HttpGet("getDeliveryPrice")]
    public async Task<IActionResult> GetDeliveryPrice(DeliveryDto? deliveryDto)
    {
        var delivery = new Delivery
        {
            OriginDelivery = new Address
            {
                PostalCode = "75384618"
            },
            DestinationDelivery = new Address
            {
                PostalCode = "74915120"
            },
            Items =
            [
                new ItemDelivery
                {
                    Weight = 2.5f
                }
            ]
        };

        var distanceOrigin = await CepHelper.SearchAddress(delivery.OriginDelivery.PostalCode);
        var distanceDestiny = await CepHelper.SearchAddress(delivery.DestinationDelivery.PostalCode);
        var distanceInKm = CepHelper.GetDistance(Convert.ToDouble(distanceOrigin.Lng, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceOrigin.Lat, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceDestiny.Lng, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceDestiny.Lat, CultureInfo.InvariantCulture));

        var getCost = delivery.CalculateDeliveryCost(distanceInKm, 2);

        return Ok(getCost);
    }

    [HttpPost]
    public async Task<IActionResult> Post(DeliveryDto dto)
    {
        var deliveryCost = CepHelper.GetDistance(
            Convert.ToDouble(dto.AddressOrigin.Longitude, CultureInfo.InvariantCulture),
            Convert.ToDouble(dto.AddressOrigin.Latitude, CultureInfo.InvariantCulture),
            Convert.ToDouble(dto.AddressDestiny.Longitude, CultureInfo.InvariantCulture),
            Convert.ToDouble(dto.AddressDestiny.Latitude, CultureInfo.InvariantCulture));

        var delivery = new Delivery
        {
            OriginDelivery = new Address
            {
                PostalCode = dto.AddressOrigin.PostalCode,
                AddressComplement = dto.AddressOrigin.AddressComplement,
                City = dto.AddressOrigin.City,
                Country = dto.AddressOrigin.Country,
                Latitude = dto.AddressOrigin.Latitude,
                Longitude = dto.AddressOrigin.Longitude,
                Neighborhood = dto.AddressOrigin.Neighborhood,
                NumberAddress = dto.AddressOrigin.NumberAddress,
                StreetAddress = dto.AddressOrigin.StreetAddress
            },
            DestinationDelivery = new Address
            {
                PostalCode = dto.AddressOrigin.PostalCode,
                AddressComplement = dto.AddressOrigin.AddressComplement,
                City = dto.AddressOrigin.City,
                Country = dto.AddressOrigin.Country,
                Latitude = dto.AddressOrigin.Latitude,
                Longitude = dto.AddressOrigin.Longitude,
                Neighborhood = dto.AddressOrigin.Neighborhood,
                NumberAddress = dto.AddressOrigin.NumberAddress,
                StreetAddress = dto.AddressOrigin.StreetAddress
            },
            DeliveryCost = Convert.ToDecimal(deliveryCost),
            DeliveryNote = "",
            DeliveryStatus = DeliveryStatus.Pending,
            Items = dto.Items.Select(x => new ItemDelivery
            {
                Name = x.Name,
                Description = x.Description,
                Weight = x.Weight
            }).ToList(),
            ScheduledTime = DateTime.Now,
            DeliveryCode = DeliveryHelper.GenerateDeliveryCode()
        };

        return Ok(delivery);
    }

    [HttpPost]
    public async Task<IActionResult> FinalizeOrder(Guid? id, string deliveryCode)
    {
        var domain = await _deliveryRepository.GetById(id.Value);

        if (domain == null)
            return BadRequest("Objeto Delivery não encontrado");

        if (!domain.DeliveryCode.Equals(deliveryCode, StringComparison.CurrentCultureIgnoreCase))
            return BadRequest("O código informado não representa ao código do delivery");

        domain.DeliveryStatus = DeliveryStatus.Ok;

        return Ok();

    }
}
