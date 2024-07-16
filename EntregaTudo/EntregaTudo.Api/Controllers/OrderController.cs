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
using MongoDB.Bson;

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

    /// <summary>
    /// Método para o entregador buscar o pedido no cliente
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="deliveryPersonId"></param>
    /// <param name="deliveryPersonCode"></param>
    /// <returns></returns>
    [HttpPost("sendOrder")]
    public async Task<IActionResult> SendOrder(ObjectId? orderId, string deliveryPersonId, string? deliveryPersonCode)
    {
        /*
         O que pensei, gerar dois codigos, um para o entregador, outro para quem irá receber, só vai mudar o status do pedido
        depois que o entregador informar o codigo do delivery, aí vamos gerar outro codigo, para assim ser finalizado e pago. 
        Caso de uso: O entregador irá buscar o pedido na casa do cliente, assim, o cliente irá informar o codigo, e o entregador irá confirmar nesse metodo, na hora que for entregar o pedido, o usuario que irá receber irá passar o codigo novo para o entregador que será finalizado o pedido.
         */
        var domain = await orderRepository.GetAsync(orderId.Value);

        if (domain == null)
            return BadRequest("Objeto Delivery não encontrado");

        if(domain.DeliveryStatus != DeliveryStatus.Pending)
            return BadRequest("Delivery não está disponível!");

        if(!domain.ConfirmDelivery(deliveryPersonCode))
            return BadRequest("O código informado não representa ao código do delivery");

        domain.DeliveryStatus = DeliveryStatus.Sended;
        domain.DeliveryPersonId = deliveryPersonId;
        domain.DeliveryCode = DeliveryHelper.GenerateDeliveryCode();

        await orderRepository.SaveOrUpdateAsync(domain);

        return Ok();

    }

    /// <summary>
    /// Método para o entregador finalizar o pedido no cliente
    /// </summary>
    /// <param name="id"></param>
    /// <param name="deliveryCode"></param>
    /// <returns></returns>
    [HttpPost("finalizeOrder")]
    public async Task<IActionResult> FinalizeOrder(ObjectId? id, string deliveryCode)
    {
        var domain = await orderRepository.GetAsync(id.Value);

        if (domain == null)
            return BadRequest("Objeto Delivery não encontrado");

        if (!domain.ConfirmDelivery(deliveryCode))
            return BadRequest("O código informado não representa ao código do delivery");

        await orderRepository.SaveOrUpdateAsync(domain);

        return Ok();

    }
}
