using EntregaTudo.Api.Controllers.Base;
using EntregaTudo.Api.Helpers;
using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Infrastructure;
using EntregaTudo.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using EntregaTudo.Core.Domain.Enum;
using EntregaTudo.Core.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EntregaTudo.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class OrderController(IWebHostEnvironment webHostEnvironment,
    ILogger<OrderController> logger,
    IServiceProvider serviceProvider,
    IOrderRepository orderRepository)
    : ApiControllerBase(webHostEnvironment, logger, serviceProvider)
{
    /// <summary>
    /// Método para calcular o preço do delivery
    /// </summary>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    [HttpPost("getDeliveryPrice")]
    public async Task<IActionResult> GetDeliveryPrice(OrderDto? orderDto)
    {
        var delivery = new Order
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
                new OrderItem
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
    public async Task<IActionResult> Post(OrderDto orderDto)
    {
        if (User.Identity is not ClaimsIdentity user) return BadRequest("User not found");

        if(user.FindFirst(ClaimTypes.NameIdentifier) == null)
            return BadRequest("User not found");

        var order = new Order
        {
            OriginDelivery = new Address
            {
                PostalCode = orderDto.AddressOrigin.PostalCode,
                AddressComplement = orderDto.AddressOrigin.AddressComplement,
                City = orderDto.AddressOrigin.City,
                Country = orderDto.AddressOrigin.Country,
                Latitude = orderDto.AddressOrigin.Latitude,
                Longitude = orderDto.AddressOrigin.Longitude,
                Neighborhood = orderDto.AddressOrigin.Neighborhood,
                NumberAddress = orderDto.AddressOrigin.NumberAddress,
                StreetAddress = orderDto.AddressOrigin.StreetAddress
            },
            DestinationDelivery = new Address
            {
                PostalCode = orderDto.AddressOrigin.PostalCode,
                AddressComplement = orderDto.AddressOrigin.AddressComplement,
                City = orderDto.AddressOrigin.City,
                Country = orderDto.AddressOrigin.Country,
                Latitude = orderDto.AddressOrigin.Latitude,
                Longitude = orderDto.AddressOrigin.Longitude,
                Neighborhood = orderDto.AddressOrigin.Neighborhood,
                NumberAddress = orderDto.AddressOrigin.NumberAddress,
                StreetAddress = orderDto.AddressOrigin.StreetAddress
            },
            DeliveryCost = orderDto.DeliveryCost.Value,
            DeliveryNote = "",
            DeliveryStatus = DeliveryStatus.Pending,
            Items = orderDto.Items.Select(x => new OrderItem
            {
                Name = x.Name,
                Description = x.Description,
                Weight = x.Weight
            }).ToList(),
            ScheduledTime = DateTime.Now,
            DeliveryCode = DeliveryHelper.GenerateDeliveryCode(),
            CustomerId = user.FindFirst(ClaimTypes.NameIdentifier).Value,
        };

        await orderRepository.SaveAsync(order);

        return Ok(order);
    }

    [HttpPost("finalizeOrder")]
    public async Task<IActionResult> FinalizeOrder(Guid? id, string deliveryCode)
    {
        var domain = await orderRepository.GetAsync(id.Value);

        if (domain == null)
            return BadRequest("Objeto Delivery não encontrado");

        if (!domain.ConfirmDelivery(deliveryCode))
            return BadRequest("O código informado não representa ao código do delivery");

        domain.DeliveryStatus = DeliveryStatus.Ok;

        return Ok();

    }
}
