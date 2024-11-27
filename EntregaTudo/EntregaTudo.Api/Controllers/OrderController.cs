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
using static EntregaTudo.Shared.Dto.OrderDetailsDto;

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
                PostalCode = orderDto.AddressOrigin.PostalCode
            },
            DestinationDelivery = new Address
            {
                PostalCode = orderDto.AddressDestiny.PostalCode
            },
            Items =
            [
                new OrderItem
                    {
                        Weight = 1
                    }
            ]
        };

        var distanceOrigin = await CepHelper.SearchAddress(delivery.OriginDelivery.PostalCode);
        var distanceDestiny = await CepHelper.SearchAddress(delivery.DestinationDelivery.PostalCode);
        var distanceInKm = CepHelper.GetDistance(Convert.ToDouble(distanceOrigin.Lng, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceOrigin.Lat, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceDestiny.Lng, CultureInfo.InvariantCulture),
            Convert.ToDouble(distanceDestiny.Lat, CultureInfo.InvariantCulture));

        var getCost = delivery.CalculateDeliveryCost(distanceInKm, 1);

        return Ok(getCost);
    }

    [HttpPost]
    public async Task<IActionResult> Post(OrderDto orderDto)
    {
        if (User.Identity is not ClaimsIdentity user) return BadRequest("User not found");

        if (user.FindFirst(ClaimTypes.NameIdentifier) == null)
            return BadRequest("User not found");

        var origin = await CepHelper.SearchAddress(orderDto.AddressOrigin.PostalCode);
        var destiny = await CepHelper.SearchAddress(orderDto.AddressDestiny.PostalCode);

        Enum.TryParse(origin.State, out State stateOrigin);
        Enum.TryParse(destiny.State, out State stateDestiny);

        var order = new Order
        {
            OriginDelivery = new Address
            {
                PostalCode = origin.PostalCode,
                AddressComplement = orderDto.AddressOrigin.AddressComplement,
                City = origin.City,
                Country = "Brasil",
                Latitude = Convert.ToDouble(origin.Lat),
                Longitude = Convert.ToDouble(origin.Lng),
                Neighborhood = origin.District,
                NumberAddress = orderDto.AddressOrigin.NumberAddress,
                StreetAddress = origin.Address,
                State = stateOrigin
            },
            DestinationDelivery = new Address
            {
                PostalCode = destiny.PostalCode,
                AddressComplement = orderDto.AddressOrigin.AddressComplement,
                City = destiny.City,
                Country = "Brasil",
                Latitude = Convert.ToDouble(destiny.Lat),
                Longitude = Convert.ToDouble(destiny.Lng),
                Neighborhood = destiny.District,
                NumberAddress = orderDto.AddressOrigin.NumberAddress,
                StreetAddress = destiny.Address,
                State = stateDestiny
            },
            DeliveryCost = orderDto.DeliveryCost.Value,
            DeliveryNote = "",
            DeliveryStatus = DeliveryStatus.Pending,
            Items = orderDto.Items.Select(o => new OrderItem
            {
                Name = o.Name,
                Description = o.Description ?? string.Empty,
                Weight = o.Weight ?? 0
            }).ToList(),
            ScheduledTime = DateTime.Now,
            DeliveryCode = DeliveryHelper.GenerateDeliveryCode(),
            CustomerId = user.FindFirst(ClaimTypes.NameIdentifier).Value,
        };

        await orderRepository.SaveAsync(order);

        return Ok(new
        {
            Id = order.Id.ToString(),
            order.DeliveryCode,
            order.DeliveryCost,
            order.DestinationDelivery,
            Items = order.Items.Select(i => new { i.Name, i.Description, i.Weight }).ToList()
        });
    }

    [HttpGet("getOrder")]
    public async Task<IActionResult> GetOrder(string id)
    {
        if (User.Identity is not ClaimsIdentity user) return BadRequest("User not found");

        if (user.FindFirst(ClaimTypes.NameIdentifier) == null)
            return BadRequest("User not found");

        var order = await orderRepository.GetAsync(id);

        return Ok(new
        {
            order.DeliveryCode,
            order.DeliveryCost,
            order.DestinationDelivery,
            Items = order.Items.Select(i => new { i.Name, i.Description, i.Weight }).ToList()
        });
    }

    /// <summary>
    /// Método para buscar os pedidos do cliente
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("orders-customer")]
    public async Task<IActionResult> AllOrdersCustomer(string id)
    {
        if (User.Identity is not ClaimsIdentity user) return BadRequest("User not found");

        if (user.FindFirst(ClaimTypes.NameIdentifier) == null)
            return BadRequest("User not found");

        var orders = orderRepository.Find(x => x.CustomerId == id).ToList();

        var order = new List<OrderDetailsDto>();

        foreach (var item in orders.OrderByDescending(x => x.ScheduledTime))
        {
            var status = item.DeliveryStatus.ToString() switch
            {
                "Ok" => "Entregue",
                "Pending" => "Aguardando Entregador",
                "Sended" => "Em Trânsito",
                _ => ""
            };

            order.Add(new OrderDetailsDto
            {
                DeliveryCode = item.DeliveryCode,
                DeliveryCost = item.DeliveryCost,
                DeliveryStatus = status,
                OrderId = item.Id.ToString(),
                DestinationDelivery = new DestinationDeliveryDto
                {
                    StreetAddress = item.DestinationDelivery.StreetAddress,
                    AddressComplement = item.DestinationDelivery.AddressComplement,
                    City = item.DestinationDelivery.City,
                    Country = item.DestinationDelivery.Country,
                    Neighborhood = item.DestinationDelivery.Neighborhood,
                    State = item.DestinationDelivery.State.ToString(),
                },
                DateHourOrder = item.ScheduledTime,
                OrderDetailsItems = item.Items.Select(s => new OrderDetailItemsDto
                {
                    Description = s.Description,
                    Name = s.Name,
                    Weight = s.Weight
                }).ToList()
            });
        }

        return Ok(new
        {
            order
        });
    }

    /// <summary>
    /// Método para buscar as entregas feitas pelo entregador
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("orders-delivery-person")]
    public async Task<IActionResult> AllOrdersDeliveryPerson(string id)
    {
        if (User.Identity is not ClaimsIdentity user) return BadRequest("User not found");

        if (user.FindFirst(ClaimTypes.NameIdentifier) == null)
            return BadRequest("User not found");

        var orders = orderRepository
            .Find(x => x.DeliveryPersonId == id && x.DeliveryStatus == DeliveryStatus.Ok).ToList();

        var order = new List<OrderDetailsDto>();

        foreach (var item in orders.OrderByDescending(x => x.ScheduledTime))
        {
            order.Add(new OrderDetailsDto
            {
                DeliveryCost = item.DeliveryCost,
                DestinationDelivery = new DestinationDeliveryDto
                {
                    StreetAddress = item.DestinationDelivery.StreetAddress,
                    AddressComplement = item.DestinationDelivery.AddressComplement,
                    City = item.DestinationDelivery.City,
                    Country = item.DestinationDelivery.Country,
                    Neighborhood = item.DestinationDelivery.Neighborhood,
                    State = item.DestinationDelivery.State.ToString(),
                },
                DateHourOrder = item.ScheduledTime,
                OrderDetailsItems = item.Items.Select(s => new OrderDetailItemsDto
                {
                    Description = s.Description,
                    Name = s.Name,
                    Weight = s.Weight
                }).ToList()
            });
        }

        return Ok(new
        {
            order
        });
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
        var domain = await orderRepository.GetAsync(orderId.Value);

        if (domain == null)
            return BadRequest("Objeto Delivery não encontrado");

        if (domain.DeliveryStatus != DeliveryStatus.Pending)
            return BadRequest("Delivery não está disponível!");

        if (!domain.ConfirmDelivery(deliveryPersonCode))
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
    public async Task<IActionResult> FinalizeOrder(ObjectId? orderId, string id, string deliveryCode)
    {
        var domain = await orderRepository.GetAsync(orderId.Value);

        if (domain == null)
            return BadRequest(new { message = "Objeto Delivery não encontrado" });

        if (!domain.ConfirmDelivery(deliveryCode))
            return BadRequest(new { message = "O código informado não representa ao código do pedido, tente novamente!" });

        domain.DeliveryPersonId = id;

        await orderRepository.SaveOrUpdateAsync(domain);

        return Ok();

    }
}
